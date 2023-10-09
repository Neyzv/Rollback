using Rollback.Common.Logging;
using Rollback.Protocol.Enums;
using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Characters;
using Rollback.World.Database.Pets;
using Rollback.World.Game.Effects.Types;
using Rollback.World.Game.Fights;
using Rollback.World.Game.Fights.Fighters;
using Rollback.World.Game.Fights.Types;
using Rollback.World.Game.Items.Attributes;
using Rollback.World.Game.Items.Storages;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Items.Types.Custom
{
    [ItemType((short)ItemType.Familier)]
    public sealed class PetItem : PlayerItem
    {
        private const byte MaxMealBeforeIndigestion = 6;
        private const byte MealsCountForBonus = 3;
        private const byte MinMaxEffectPod = 80;

        private static readonly IReadOnlyDictionary<EffectId, double> _effectPods =
            new Dictionary<EffectId, double>()
            {
                [EffectId.EffectIncreaseWeight] = 0.1,
                [EffectId.EffectAddVitality] = 1,
                [EffectId.EffectAddChance] = 1,
                [EffectId.EffectAddAgility] = 1,
                [EffectId.EffectAddStrength] = 1,
                [EffectId.EffectAddIntelligence] = 1,
                [EffectId.EffectAddProspecting] = 1,
                [EffectId.EffectIncreaseDamage138] = 2,
                [EffectId.EffectAddWisdom] = 3,
                [EffectId.EffectAddAirResistPercent] = 4,
                [EffectId.EffectAddWaterResistPercent] = 4,
                [EffectId.EffectAddFireResistPercent] = 4,
                [EffectId.EffectAddEarthResistPercent] = 4,
                [EffectId.EffectAddNeutralResistPercent] = 4,
                [EffectId.EffectAddHealBonus] = 8,
                [EffectId.EffectAddDamageBonus] = 8,
                [EffectId.EffectAddDamageBonus121] = 8,
                [EffectId.EffectAddDamageReflection] = 8,
                [EffectId.EffectAddDamageReflection220] = 8,
            };

        private readonly PetRecord? _petRecord;

        private bool _isDead;
        private short _actualPod;

        private EffectDice? _corpulenceEffect;
        private EffectDice CorpulenceEffect =>
            _corpulenceEffect ??= Effects.OfType<EffectDice>().First(x => x.Id is EffectId.EffectCorpulence);

        private EffectInteger? _lastMealEffect;
        private EffectInteger LastMealEffect =>
            _lastMealEffect ??= Effects.OfType<EffectDice>().First(x => x.Id is EffectId.EffectLastMeal);

        private EffectInteger? _mealCountEffect;
        private EffectInteger MealCountEffect =>
            _mealCountEffect ??= Effects.OfType<EffectDice>().First(x => x.Id is EffectId.EffectMealCount);

        private EffectDate? _lastMealDateEffect;
        private EffectDate LastMealDateEffect =>
            _lastMealDateEffect ??= Effects.OfType<EffectDate>().First(x => x.Id is EffectId.EffectLastMealDate);

        private EffectDice? _lifePointsEffect;
        private EffectDice? LifePointsEffect =>
            _lifePointsEffect ??= Effects.OfType<EffectDice>().FirstOrDefault(x => x.Id is EffectId.EffectLifePoints);

        private EffectInteger? _boostedEffect;
        private EffectInteger? BoostedEffect =>
            _boostedEffect ??= Effects.OfType<EffectInteger>().FirstOrDefault(x => x.Id is EffectId.EffectIncreasePetStats);

        public bool IsBoosted =>
            BoostedEffect is not null;

        public short LifePoints
        {
            get => LifePointsEffect!.Value;
            set
            {
                LifePointsEffect!.Value = value;

                if (LifePointsEffect.Value <= 0)
                    Die();
                else if (LifePointsEffect.Value > LifePointsEffect.DiceFace)
                    LifePointsEffect.Value = LifePointsEffect.DiceFace;
            }
        }

        private short Corpulence
        {
            get =>
                CorpulenceEffect.DiceNum > 0 ?
                    CorpulenceEffect.DiceNum
                : (short)(CorpulenceEffect.DiceFace > 0 ? -CorpulenceEffect.DiceFace : 0);
            set
            {
                if (value < -100)
                    value = -100;

                (_corpulenceEffect ??= Effects.OfType<EffectDice>().First(x => x.Id is EffectId.EffectCorpulence)).DiceFace = (short)(value > 0 ? value : 0);
                _corpulenceEffect.DiceNum = (short)(value < 0 ? -value : 0);
            }
        }

        public short? BoostItemId =>
            _petRecord?.BoostItemId;

        public override ObjectItem ObjectItem
        {
            get
            {
                UpdateCorpulence();

                return base.ObjectItem;
            }
        }

        public PetItem(CharacterItemRecord record, Inventory inventory)
            : base(record, inventory)
        {
            _petRecord = ItemManager.Instance.GetPetRecord(Id);

            if (_petRecord is not null)
            {
                Created += OnPetCreated;
                Equipped += OnPetEquipped;
                Unequipped += OnPetUnequipped;

                UpdateCorpulence();

                if (_petRecord.MaxEffectPod.HasValue == false)
                    CalculateEffectsPodsInformations();

                foreach (var effect in Effects.OfType<EffectInteger>())
                {
                    if (_petRecord.FoodInformations.TryGetValue(effect.Id, out var foodInformation))
                        _actualPod += (short)(effect.Value * foodInformation.EffectBasePod!);
                }
            }
        }

        private void UpdateCorpulence()
        {
            if (LifePointsEffect is not null && CheckMaxMealDate(DateTime.Now))
                Corpulence--;
        }

        private bool CheckMaxMealDate(DateTime now) =>
            _petRecord?.MaxMealHours.HasValue == true &&
                    (now - LastMealDateEffect.Date).TotalHours > _petRecord.MaxMealHours;

        private void OnPetEquipped(PlayerItem _)
        {
            _storage.Owner.Dismount();
            _storage.Owner.FightEnded += OnFightEnded;
        }

        private void OnPetUnequipped(PlayerItem _) =>
            _storage.Owner.FightEnded -= OnFightEnded;

        private void OnFightEnded(IFight fight)
        {
            if (_petRecord is not null)
            {
                if (fight.Winners.Any(x => x.Id == _storage.Owner.Id))
                {
                    if (fight is FightPvM fightPvM)
                        foreach (var monster in fightPvM.Losers.OfType<MonsterFighter>())
                        {
                            foreach (var foodInformations in _petRecord.FoodInformations.Values.Where(x => x.FoodType is FoodType.Monsters))
                            {
                                if (foodInformations.FoodInformations.ContainsKey(monster.MonsterId))
                                {
                                    if (Effects.OfType<EffectDice>()
                                        .FirstOrDefault(x => x.Id is EffectId.EffectMonsterKilledCount && x.DiceNum == monster.MonsterId) is { } effect)
                                    {
                                        effect.Value++;

                                        if (effect.Value < 0)
                                            effect.Value = short.MaxValue;

                                        if (foodInformations.FoodInformations.TryGetValue(effect.DiceNum, out var killNeeded) &&
                                            effect.Value % killNeeded == 0)
                                            IncreaseBonus(foodInformations);
                                    }
                                    else
                                        Effects.Add(new EffectDice()
                                        {
                                            Id = EffectId.EffectMonsterKilledCount,
                                            DiceNum = monster.MonsterId,
                                            Value = 1
                                        });
                                }
                            }
                        }
                }
                else
                    LifePoints--;

                _storage.RefreshItem(UID);
            }
        }

        public override bool Feed(PlayerItem item)
        {
            var result = true;

            if (_petRecord is not null && _petRecord.MinMealHours.HasValue)
            {
                var now = DateTime.Now;
                var effectsToBoost = _petRecord.FoodInformations.Values.Where(x => (x.FoodType is FoodType.Items && x.FoodInformations.ContainsKey(item.Id)) ||
                    (x.FoodType is FoodType.ItemsCategories && x.FoodInformations.ContainsKey((short)item.TypeId)))
                    .ToArray();

                if (effectsToBoost.Length is not 0)
                {
                    // "Votre familier apprécie le repas."
                    short replyMessageId = 32;
                    var increaseMealCount = true;

                    if (CheckMaxMealDate(now))
                    {
                        // Vous donnez à manger à votre familier famélique qui traînait comme un zombi. Il se force à manger mais la nourriture qu\'il avale fait 3 fois son estomac et il se tord de douleur. Au moins il a mangé.
                        replyMessageId = 31;
                        increaseMealCount = false;
                    }
                    else if (Corpulence < 0)
                    {
                        Corpulence++;
                        // "Vous donnez à manger à votre familier. Il semble qu\'il avait très faim."
                        replyMessageId = 29;

                        increaseMealCount = false;
                    }
                    else if ((now - LastMealDateEffect.Date).TotalHours < _petRecord.MinMealHours)
                    {
                        Corpulence++;

                        if (Corpulence > MaxMealBeforeIndigestion)
                        {
                            LifePoints--;
                            // "Vous donnez à manger à répétition à votre familier déjà obèse. Il avale quand même la ressource et fait une indigestion.";
                            replyMessageId = 27;
                            increaseMealCount = false;
                        }
                        else
                            // "Vous donnez à manger à votre familier alors qu\'il n\'avait plus faim. Il se force pour vous faire plaisir"
                            replyMessageId = 26;
                    }

                    if (increaseMealCount && ++MealCountEffect.Value % MealsCountForBonus == 0)
                        foreach (var effectToBoost in effectsToBoost)
                            IncreaseBonus(effectToBoost);

                    LastMealDateEffect.SetDate(now);
                    LastMealEffect.Value = item.Id;

                    _storage.RefreshItem(UID);
                    _storage.Owner.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, replyMessageId);
                }
                else
                    result = false;
            }
            else
                result = false;

            return result;
        }

        private void CalculateEffectsPodsInformations()
        {
            if (_petRecord is not null)
            {
                _petRecord.MaxEffectPod = MinMaxEffectPod;
                _petRecord.MaxEffectPodBoosted = MinMaxEffectPod;

                foreach (var foodInformations in _petRecord.FoodInformations.Values)
                    if (_effectPods.TryGetValue(foodInformations.Effect.Id, out var pod))
                    {
                        if (foodInformations.Effect is EffectInteger effectInteger)
                        {
                            var boostedEffectPod = (short)(effectInteger.Value * pod);
                            if (boostedEffectPod > _petRecord.MaxEffectPod)
                                _petRecord.MaxEffectPod = boostedEffectPod;
                        }

                        var effectBoostedPod = (short)(foodInformations.BoostedValue * pod);
                        if (effectBoostedPod > _petRecord.MaxEffectPodBoosted)
                            _petRecord.MaxEffectPodBoosted = effectBoostedPod;
                    }
                    else
                    {
                        _storage.Owner.Client.Dispose();
                        Logger.Instance.LogWarn($"Unhandled effect id pod {foodInformations.Effect.Id} for pets, Id {Id}...");
                        return;
                    }

                foreach (var foodInformations in _petRecord.FoodInformations.Values)
                {
                    if (foodInformations.Effect is EffectInteger effectInteger &&
                        _effectPods.TryGetValue(effectInteger.Id, out var pod))
                    {
                        foodInformations.EffectBasePod = (short)Math.Ceiling(_petRecord.MaxEffectPod.Value / effectInteger.Value * (1 - pod / 100d));
                        foodInformations.EffectBasePodBoosted = (short)Math.Ceiling(_petRecord.MaxEffectPod.Value / foodInformations.BoostedValue * (1 - pod / 100d));
                    }
                }
            }
        }

        private void IncreaseBonus(PetFoodRecord foodInformations)
        {
            if (_petRecord is not null && Effects.OfType<EffectInteger>()
                .FirstOrDefault(x => x.Id == foodInformations.Effect.Id) is { } effect)
            {
                var increasedEffectPod = (short)((IsBoosted ? foodInformations.EffectBasePodBoosted! : foodInformations.EffectBasePod!)
                    * foodInformations.StatIncreaseAmount);

                if (foodInformations.Effect is EffectInteger effectInteger &&
                    effect.Value < effectInteger.Value &&
                    _actualPod + increasedEffectPod < _petRecord.MaxEffectPod)
                {
                    effect.Value += foodInformations.StatIncreaseAmount;
                    _actualPod += increasedEffectPod;
                }
            }
            else
                Effects.Add(new EffectInteger()
                {
                    Id = foodInformations.Effect.Id,
                    Value = foodInformations.StatIncreaseAmount
                });
        }

        private void OnPetCreated(PlayerItem item)
        {
            if (_petRecord is not null && LifePointsEffect is null)
            {
                _lifePointsEffect = new EffectDice()
                {
                    Id = EffectId.EffectLifePoints,
                    DiceFace = _petRecord.MaxLifePoints,
                    Value = _petRecord.MaxLifePoints
                };
                item.Effects.Add(_lifePointsEffect);

                if (_petRecord.FoodInformations.Values.Any(x => x.FoodType is FoodType.Items || x.FoodType is FoodType.ItemsCategories))
                {
                    _corpulenceEffect = new EffectDice()
                    {
                        Id = EffectId.EffectCorpulence
                    };
                    item.Effects.Add(_corpulenceEffect);

                    _lastMealEffect = new EffectInteger()
                    {
                        Id = EffectId.EffectLastMeal
                    };
                    item.Effects.Add(_lastMealEffect);

                    _lastMealDateEffect = new EffectDate()
                    {
                        Id = EffectId.EffectLastMealDate,
                    };
                    _lastMealDateEffect.SetDate(DateTime.Now);
                    Effects.Add(_lastMealDateEffect);

                    _mealCountEffect = new EffectInteger()
                    {
                        Id = EffectId.EffectMealCount,
                        Value = 0
                    };
                    item.Effects.Add(_mealCountEffect);
                }
            }

            Created -= OnPetCreated;
        }

        public override void UpdateItemSkin(Character owner)
        {
            if (IsEquipped)
                owner.CharacterLook.SetSubLook(new(0, SubEntityBindingPointCategoryEnum.HOOK_POINT_CATEGORY_PET, new(AppearanceId, new(), new(), new(), new())));
            else
                owner.CharacterLook.SubLooks.RemoveAll(x => x.Category == SubEntityBindingPointCategoryEnum.HOOK_POINT_CATEGORY_PET);

            owner.RefreshLook();
        }

        public void Die()
        {
            if (!_isDead && _petRecord is not null)
            {
                if (ItemManager.Instance.GetTemplateRecordById(_petRecord.GhostId) is { } ghostTemplate)
                {
                    var itemQuantity = Quantity;

                    LifePointsEffect!.Value = 0;
                    _storage.RemoveItem(this, itemQuantity);
                    _storage.AddItem(ItemManager.Instance.CreatePlayerItem(_storage, ghostTemplate, itemQuantity, EffectGenerationType.Normal, Effects));

                    _isDead = true;
                }
                else
                {
                    _storage.Owner.SendServerMessage($"Pet {Id} died but ghost item id {_petRecord.GhostId} doesn't exist...");
                    LifePoints = 1;
                }
            }
        }

        public bool Boost()
        {
            var result = false;

            if (!IsBoosted && (result = true))
            {
                _boostedEffect = new EffectInteger()
                {
                    Id = EffectId.EffectIncreasePetStats
                };
                Effects.Add(_boostedEffect);

                _storage.RefreshItem(UID);
            }

            return result;
        }
    }
}
