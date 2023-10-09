using System.Reflection;
using Rollback.Common.DesignPattern.Assemblies;
using Rollback.Common.DesignPattern.Attributes;
using Rollback.Common.DesignPattern.Instance;
using Rollback.Common.Initialization;
using Rollback.Common.IO.Binary;
using Rollback.Common.Logging;
using Rollback.Common.ORM;
using Rollback.Protocol.Enums;
using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Effects;
using Rollback.World.Game.Effects.Handlers.Items;
using Rollback.World.Game.Effects.Handlers.Items.Stats;
using Rollback.World.Game.Effects.Handlers.Items.Usable;
using Rollback.World.Game.Effects.Handlers.Spells;
using Rollback.World.Game.Fights;
using Rollback.World.Game.Fights.Fighters;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.World.Maps;
using Rollback.World.Game.World.Maps.CellsZone;
using Rollback.World.Game.World.Maps.CellsZone.Shapes;
using Single = Rollback.World.Game.World.Maps.CellsZone.Shapes.Single;

namespace Rollback.World.Game.Effects
{
    public sealed class EffectManager : Singleton<EffectManager>
    {
        private readonly Dictionary<int, EffectRecord> _records;
        private readonly Dictionary<EffectId, Func<EffectBase, Character, ItemEffectHandler>> _itemEffectHandlers;
        private readonly Dictionary<EffectId, Func<EffectBase, List<FightActor>, SpellCast, Zone, SpellEffectHandler>> _spellEffectHandlers;
        private readonly Dictionary<EffectId, Func<EffectBase, Character, Cell, UsableEffectHandler>> _usableEffectHandlers;

        private static readonly IReadOnlyCollection<EffectId> _diceEffects = new HashSet<EffectId>()
        {
            EffectId.EffectDamageWater,
            EffectId.EffectDamageEarth,
            EffectId.EffectDamageAir,
            EffectId.EffectDamageFire,
            EffectId.EffectDamageNeutral,

            EffectId.EffectStealHPWater,
            EffectId.EffectStealHPEarth,
            EffectId.EffectStealHPAir,
            EffectId.EffectStealHPFire,
            EffectId.EffectStealHPNeutral,

            EffectId.EffectLostAP,

            EffectId.EffectRemainingFights,

            EffectId.EffectHealHP108,

            EffectId.EffectSoulStone,
            EffectId.EffectSoulStoneSummon,

            EffectId.EffectExchangeable,

            EffectId.EffectLastMeal,
            EffectId.EffectLastMealDate,

            EffectId.EffectCorpulence,

            EffectId.EffectAddXp,
            EffectId.EffectJobXp,
        };

        /// <summary>
        /// Key : Effect | Value : reflect if buff
        /// </summary>
        private static readonly IReadOnlyDictionary<EffectId, bool> _reflectableEffects = new Dictionary<EffectId, bool>()
        {
            [EffectId.EffectDamageEarth] = false,
            [EffectId.EffectDamageAir] = false,
            [EffectId.EffectDamageFire] = false,
            [EffectId.EffectDamageNeutral] = false,
            [EffectId.EffectDamageWater] = false,

            [EffectId.EffectStealHPAir] = false,
            [EffectId.EffectStealHPEarth] = false,
            [EffectId.EffectStealHPFire] = false,
            [EffectId.EffectStealHPFix] = false,
            [EffectId.EffectStealHPNeutral] = false,
            [EffectId.EffectStealHPWater] = false,

            [EffectId.EffectDamagePercentAir] = false,
            [EffectId.EffectDamagePercentEarth] = false,
            [EffectId.EffectDamagePercentFire] = false,
            [EffectId.EffectDamagePercentWater] = false,
            [EffectId.EffectDamagePercentNeutral] = false,
            [EffectId.EffectDamagePercentNeutral671] = false,

            [EffectId.EffectPunishmentDamage] = false,

            [EffectId.EffectSubAP] = true,
            [EffectId.EffectLostAP] = true,
            [EffectId.EffectSubMP] = true,
            [EffectId.EffectLostMP] = true,
        };

        private static readonly IReadOnlyDictionary<Characteristic, Stat> _effectsRelation = new Dictionary<Characteristic, Stat>()
        {
            { Characteristic.Ap, Stat.ActionPoints },
            { Characteristic.Mp, Stat.MovementPoints },
            { Characteristic.Range, Stat.Range },
            { Characteristic.Summons, Stat.SummonableCreaturesBoost },
            { Characteristic.Prospecting, Stat.Prospecting },
            { Characteristic.Weight, Stat.Weight },
            { Characteristic.Heals, Stat.HealBonus },
            { Characteristic.Initiative, Stat.Initiative },
            { Characteristic.Intelligence, Stat.Intelligence },
            { Characteristic.Agility, Stat.Agility },
            { Characteristic.Chance, Stat.Chance },
            { Characteristic.Vitality, Stat.Vitality },
            { Characteristic.Strength, Stat.Strength },
            { Characteristic.Wisdom, Stat.Wisdom },
            { Characteristic.Damage, Stat.AllDamagesBonus },
            { Characteristic.DamagePercent, Stat.DamagesBonusPercent },
            { Characteristic.TrapsBonusFixed, Stat.TrapBonus },
            { Characteristic.TrapsBonusPercent, Stat.TrapBonusPercent },
            { Characteristic.WeaponSkill, Stat.WeaponDamagesBonusPercent },
            { Characteristic.Reflect, Stat.Reflect },
            { Characteristic.CriticalHits, Stat.CriticalHit },
            { Characteristic.CriticalFailure, Stat.CriticalMiss },
            { Characteristic.ApLossRes, Stat.DodgeApLostProbability },
            { Characteristic.MpLossRes, Stat.DodgeMpLostProbability },
            { Characteristic.NeutralReductionPercent, Stat.NeutralElementResistPercent },
            { Characteristic.EarthReductionPercent, Stat.EarthElementResistPercent },
            { Characteristic.FireReductionPercent, Stat.FireElementResistPercent },
            { Characteristic.WaterReductionPercent, Stat.WaterElementResistPercent },
            { Characteristic.AirReductionPercent, Stat.AirElementResistPercent },
            { Characteristic.NeutralReductionPercentPvP, Stat.PvpNeutralElementResistPercent },
            { Characteristic.EarthReductionPercentPvP, Stat.PvpEarthElementResistPercent },
            { Characteristic.FireReductionPercentPvP, Stat.PvpFireElementResistPercent },
            { Characteristic.WaterReductionPercentPvP, Stat.PvpWaterElementResistPercent },
            { Characteristic.AirReductionPercentPvP, Stat.PvpAirElementResistPercent },
            { Characteristic.AirReductionFixed, Stat.AirElementReduction },
            { Characteristic.FireReductionFixed, Stat.FireElementReduction },
            { Characteristic.WaterReductionFixed, Stat.WaterElementReduction },
            { Characteristic.NeutralReductionFixed, Stat.NeutralElementReduction },
            { Characteristic.EarthReductionFixed, Stat.EarthElementReduction },
            { Characteristic.AirReductionFixedPvP, Stat.PvpAirElementReduction },
            { Characteristic.EarthReductionFixedPvP, Stat.PvpEarthElementReduction },
            { Characteristic.NeutralReductionFixedPvP, Stat.PvpNeutralElementReduction },
            { Characteristic.FireReductionFixedPvP, Stat.PvpFireElementReduction },
            { Characteristic.WaterReductionFixedPvP, Stat.PvpWaterElementReduction },
        };

        private static readonly IReadOnlyCollection<EffectId> _triggerMarkEffects = new HashSet<EffectId>()
        {
            EffectId.EffectGlyph,
            EffectId.EffectGlyph402,
            EffectId.EffectTrap,
        };

        private static readonly IReadOnlyCollection<EffectId> _dispellInvisibilityEffects = new HashSet<EffectId>()
        {
            EffectId.EffectDamageEarth,
            EffectId.EffectDamageAir,
            EffectId.EffectDamageFire,
            EffectId.EffectDamageNeutral,
            EffectId.EffectDamageWater,

            EffectId.EffectStealHPAir,
            EffectId.EffectStealHPEarth,
            EffectId.EffectStealHPFire,
            EffectId.EffectStealHPFix,
            EffectId.EffectStealHPNeutral,
            EffectId.EffectStealHPWater,

            EffectId.EffectDamagePercentAir,
            EffectId.EffectDamagePercentEarth,
            EffectId.EffectDamagePercentFire,
            EffectId.EffectDamagePercentWater,
            EffectId.EffectDamagePercentNeutral,
            EffectId.EffectDamagePercentNeutral671,
        };

        public EffectManager()
        {
            _records = new();
            _itemEffectHandlers = new();
            _spellEffectHandlers = new();
            _usableEffectHandlers = new();
        }

        [Initializable(InitializationPriority.DatasManager, "Effect")]
        public void Initialize()
        {
            Logger.Instance.Log("\tLoading records...");
            foreach (var record in DatabaseAccessor.Instance.Select<EffectRecord>(EffectRelator.GetEffects))
                _records[record.Id] = record;

            Logger.Instance.Log("\tLoading handlers...");

            var itemEffectHandlerType = typeof(ItemEffectHandler);
            var spellEffectHandlerType = typeof(SpellEffectHandler);
            var usableEffectHandlerType = typeof(UsableEffectHandler);
            foreach (var (type, attributes) in from assembly in AssemblyManager.Instance.Assemblies
                                               from type in assembly.GetTypes()
                                               let attributes = type.GetCustomAttributes<IdentifierAttribute>()
                                               where attributes.Any() && type.IsClass && !type.IsAbstract
                                               select (type, attributes))
            {
                if (type.IsSubclassOf(itemEffectHandlerType))
                {
                    var constructor = type.GetConstructor(new[] { typeof(EffectBase), typeof(Character) });

                    if (constructor is not null)
                    {
                        var itemEffectHandlerFactory = (EffectBase effect, Character character) =>
                            (ItemEffectHandler)constructor.Invoke(new object[] { effect, character });

                        foreach (var attribute in attributes)
                            if (attribute.Identifier is EffectId effectId)
                                if (!_itemEffectHandlers.ContainsKey(effectId))
                                    _itemEffectHandlers[effectId] = itemEffectHandlerFactory;
                                else
                                    Logger.Instance.LogError(msg: $"Found two ItemEffectHandler for effect {effectId}...");
                    }
                    else
                        Logger.Instance.LogError(msg: $"Can not find a valid constructor for {type.Name}...");
                }
                else if (type.IsSubclassOf(spellEffectHandlerType))
                {
                    var constructor = type.GetConstructor(new[] { typeof(EffectBase), typeof(List<FightActor>), typeof(SpellCast), typeof(Zone) });

                    if (constructor is not null)
                    {
                        var spellEffectHandlerFactory = (EffectBase effect, List<FightActor> target, SpellCast cast, Zone zone) =>
                            (SpellEffectHandler)constructor.Invoke(new object[] { effect, target, cast, zone });

                        foreach (var attribute in attributes)
                            if (attribute.Identifier is EffectId effectId)
                                if (!_spellEffectHandlers.ContainsKey(effectId))
                                    _spellEffectHandlers[effectId] = spellEffectHandlerFactory;
                                else
                                    Logger.Instance.LogError(msg: $"Found two SpellEffectHandler for effect {effectId}...");
                    }
                    else
                        Logger.Instance.LogError(msg: $"Can not find a valid constructor for {type.Name}...");
                }
                else if (type.IsSubclassOf(usableEffectHandlerType))
                {
                    var constructor = type.GetConstructor(new[] { typeof(EffectBase), typeof(Character), typeof(Cell) });

                    if (constructor is not null)
                    {
                        var usableEffectHandlerFactory = (EffectBase effect, Character itemOwner, Cell cell) =>
                            (UsableEffectHandler)constructor.Invoke(new object[] { effect, itemOwner, cell });

                        foreach (var attribute in attributes)
                            if (attribute.Identifier is EffectId effectId)
                                if (!_spellEffectHandlers.ContainsKey(effectId))
                                    _usableEffectHandlers[effectId] = usableEffectHandlerFactory;
                                else
                                    Logger.Instance.LogError(msg: $"Found two UsableEffectHandler for effect {effectId}...");
                    }
                    else
                        Logger.Instance.LogError(msg: $"Can not find a valid constructor for {type.Name}...");
                }
            }
        }

        public static EffectSchool GetEffectSchool(EffectId effect) =>
            effect switch
            {
                EffectId.EffectDamagePercentWater or EffectId.EffectDamageWater or EffectId.EffectStealHPWater or
                EffectId.EffectDamageWaterPerHPLost =>
                    EffectSchool.Water,

                EffectId.EffectDamagePercentEarth or EffectId.EffectDamageEarth or EffectId.EffectStealHPEarth or
                EffectId.EffectDamageEarthPerHPLost =>
                    EffectSchool.Earth,

                EffectId.EffectDamagePercentAir or EffectId.EffectDamageAir or EffectId.EffectStealHPAir or
                EffectId.EffectDamageAirPerHPLost =>
                    EffectSchool.Air,

                EffectId.EffectDamagePercentFire or EffectId.EffectDamageFire or EffectId.EffectStealHPFire or
                EffectId.EffectDamageFirePerHPLost =>
                    EffectSchool.Fire,

                EffectId.EffectDamageNeutral or EffectId.EffectDamageNeutralPerHPLost or EffectId.EffectDamagePercentNeutral or
                EffectId.EffectDamagePercentNeutral671 or EffectId.EffectStealHPNeutral or EffectId.EffectStealHPFix =>
                    EffectSchool.Neutral,

                _ => EffectSchool.Uknown
            };

        public static Stat? GetStatByCharacteristic(Characteristic characteristic) =>
            _effectsRelation.ContainsKey(characteristic) ? _effectsRelation[characteristic] : default;

        public static bool IsDiceEffect(EffectId effectId) =>
            _diceEffects.Contains(effectId);

        public static bool IsEffectReflectable(EffectBase effect) =>
            _reflectableEffects.ContainsKey(effect.Id) && (_reflectableEffects[effect.Id] || effect.Duration is 0);

        public static bool IsTriggerMarkEffect(EffectId effectId) =>
            _triggerMarkEffects.Contains(effectId);

        public static bool EffectDispellInvisibility(EffectId effectId) =>
            _dispellInvisibilityEffects.Contains(effectId);

        private static Zone GetEffectZone(Map map, Cell castCell, Cell targetedCell, EffectBase effect) =>
            effect.Shape switch
            {
                SpellShape.C => new Lozenge(map, targetedCell, effect.ZoneSize, castCell.Point.OrientationTo(targetedCell.Point)),
                SpellShape.L => new Line(map, targetedCell, effect.ZoneSize, castCell.Point.OrientationTo(targetedCell.Point), false),
                SpellShape.X => new Cross(map, targetedCell, effect.ZoneSize, castCell.Point.OrientationTo(targetedCell.Point), false, false, false),
                SpellShape.T => new Cross(map, targetedCell, effect.ZoneSize, castCell.Point.OrientationTo(targetedCell.Point), true, false, false),
                SpellShape.plus => new Cross(map, targetedCell, effect.ZoneSize, castCell.Point.OrientationTo(targetedCell.Point), false, true, false),
                SpellShape.star => new Cross(map, targetedCell, effect.ZoneSize, castCell.Point.OrientationTo(targetedCell.Point), false, false, true),
                SpellShape.minus => new Cross(map, targetedCell, effect.ZoneSize, castCell.Point.OrientationTo(targetedCell.Point), true, true, false),
                _ => new Single(map, targetedCell, effect.ZoneSize, castCell.Point.OrientationTo(targetedCell.Point))
            };

        public void HandleItemEffect(Character target, EffectBase effect, bool apply)
        {
            var handler = _itemEffectHandlers.TryGetValue(effect.Id, out var effectHandler) ? effectHandler(effect, target)
                : new StatsEffectHandler(effect, target);

            if (apply)
                handler.Apply();
            else
                handler.UnApply();
        }

        public UsableEffectHandler? HandleUsableItemEffect(Character itemOwner, EffectBase effect, Cell targetedCell)
        {
            if (_usableEffectHandlers.TryGetValue(effect.Id, out var handler))
                return handler(effect, itemOwner, targetedCell);
            else
                Logger.Instance.LogWarn($"Can not find an UsableEffectHandler for effect {effect.Id}...");

            return default;
        }

        public static ObjectEffect[] GetObjectEffects(IEnumerable<EffectBase> effects)
        {
            var dice = new List<ObjectEffectDice>();
            var integer = new List<ObjectEffectInteger>();
            var others = new List<ObjectEffect>();

            foreach (var effect in effects)
            {
                var objEffect = effect.ObjectEffect;

                if (objEffect is ObjectEffectDice objDice)
                    dice.Add(objDice);
                else if (objEffect is ObjectEffectInteger objInteger)
                    integer.Add(objInteger);
                else
                    others.Add(objEffect);
            }

            var result = new List<ObjectEffect>();
            result.AddRange(dice.OrderByDescending(x => x.diceSide).ThenBy(x => x.actionId));
            result.AddRange(others);
            result.AddRange(integer.OrderByDescending(x => x.value).ThenBy(x => x.actionId));

            return result.ToArray();
        }

        private static List<FightActor> GetAffectedActors(FightActor caster, EffectBase effect, Zone zone)
        {
            var affectedActors = new List<FightActor>();

            foreach (var affectedCell in zone.AffectedCells.Values
                .OrderBy(x => x.Point.ManhattanDistanceTo(zone.CenterCell.Point))
                .ThenBy(x => Math.Atan2(zone.CenterCell.Point.Y - x.Point.Y, x.Point.X - zone.CenterCell.Point.X)))
            {
                var finalTarget = GetRightTarget(caster.Team!.Fight.GetFighter<FightActor>(x => x.Alive && x.Cell.Id == affectedCell.Id && x.GetCarryingActor() is null), caster, effect);

                if (finalTarget is not null)
                    affectedActors.Add(finalTarget);
            }

            return affectedActors;
        }

        public static FightActor? GetRightTarget(FightActor? targetedActor, FightActor caster, EffectBase effect)
        {
            var finded = false;
            var finalTarget = default(FightActor?);

            if (targetedActor is not null)
            {
                if ((effect.TargetType.HasFlag(SpellTargetType.Self) && (finded = true) && targetedActor.Id == caster.Id) ||
                    (effect.TargetType.HasFlag(SpellTargetType.EnemyCorruption) && (finded = true) && !targetedActor.IsFriendlyWith(caster)) ||

                // Summons
                (!effect.TargetType.HasFlag(SpellTargetType.AllyExceptSummons) ?
                    ((effect.TargetType.HasFlag(SpellTargetType.AllyMonsterSummon) && (finded = true) && targetedActor.Id != caster.Id && targetedActor.IsFriendlyWith(caster) && targetedActor is SummonedMonster) ||
                        (effect.TargetType.HasFlag(SpellTargetType.AllySummon) && (finded = true) && targetedActor.Id != caster.Id && targetedActor.IsFriendlyWith(caster) && targetedActor is SummonedFighter)) :
                    ((finded = true) && targetedActor.Id != caster.Id && targetedActor.IsFriendlyWith(caster) && targetedActor is not SummonedFighter)) ||

                (!effect.TargetType.HasFlag(SpellTargetType.AllySummonsAndEnnemies) ?
                    ((effect.TargetType.HasFlag(SpellTargetType.AllyNonMonsterSummon) && (finded = true) && targetedActor.Id != caster.Id && targetedActor.IsFriendlyWith(caster) && targetedActor is SummonedFighter) ||
                        (effect.TargetType.HasFlag(SpellTargetType.EnemyMonsterSummon) && (finded = true) && !targetedActor.IsFriendlyWith(caster) && targetedActor is SummonedMonster) ||
                        ((effect.TargetType.HasFlag(SpellTargetType.EnemySummon) || effect.TargetType.HasFlag(SpellTargetType.EnemyNonMonsterSummon)) && (finded = true) && !targetedActor.IsFriendlyWith(caster) && targetedActor is SummonedFighter)) :
                    ((finded = true) && (!targetedActor.IsFriendlyWith(caster) || targetedActor is SummonedFighter))) ||

                //TO DO Static Summons

                (effect.TargetType.HasFlag(SpellTargetType.AllyMonster) && (finded = true) && targetedActor.Id != caster.Id && targetedActor.IsFriendlyWith(caster) && targetedActor is MonsterFighter) ||
                (effect.TargetType.HasFlag(SpellTargetType.EnemyMonster) && (finded = true) && !targetedActor.IsFriendlyWith(caster) && targetedActor is MonsterFighter) ||

                // Summoner
                (effect.TargetType.HasFlag(SpellTargetType.AllySummoner) && (finded = true) && caster is SummonedFighter summon && summon.Summoner.Id == targetedActor.Id) ||

                // Players
                (effect.TargetType.HasFlag(SpellTargetType.AllyPlayer) && (finded = true) && targetedActor.Id != caster.Id && targetedActor.IsFriendlyWith(caster) && targetedActor is CharacterFighter) ||
                ((effect.TargetType.HasFlag(SpellTargetType.EnemyPlayer) || effect.TargetType.HasFlag(SpellTargetType.EnemyUnkn_1)) && (finded = true) && !targetedActor.IsFriendlyWith(caster) && targetedActor is CharacterFighter))
                    finalTarget = targetedActor;
            }

            if (!finded)
            {
                if (effect.TargetType.HasFlag(SpellTargetType.SelfOnly))
                    finalTarget = caster;
                else if (targetedActor is not null)
                    Logger.Instance.LogWarn($"Unhandled target type {effect.TargetType}...");
            }

            return finalTarget;
        }

        public SpellEffectHandler[] GenerateSpellEffectHandler(SpellCast cast)
        {
            var spellEffectHandlers = new List<SpellEffectHandler>();

            var effects = cast.Critical is FightSpellCastCriticalEnum.CRITICAL_HIT ? cast.Spell.CriticalEffects : cast.Spell.Effects;

            var totalRandSum = effects.Sum(x => x.Random);
            var randFinded = false;
            var rand = Random.Shared.NextDouble();

            for (var i = 0; i < effects.Length && cast.Caster.Team!.Fight.Started; i++)
            {
                if (effects[i].Random > 0)
                {
                    if (randFinded)
                        continue;

                    var randPod = effects[i].Random / (double)totalRandSum;
                    if (rand > randPod)
                    {
                        rand -= randPod;
                        continue;
                    }

                    randFinded = true;
                }

                var zone = GetEffectZone(cast.Caster.Team!.Fight.Map.Map, cast.Caster.Cell, cast.TargetedCell, effects[i]);

                if (_spellEffectHandlers.ContainsKey(effects[i].Id))
                    spellEffectHandlers.Add(_spellEffectHandlers[effects[i].Id](effects[i], GetAffectedActors(cast.Caster, effects[i], zone), cast, zone));
                else
                    Logger.Instance.LogWarn($"Can not find a spell handler for effect {effects[i].Id}...");
            }

            return spellEffectHandlers.ToArray();
        }

        public static byte[] SerializeEffects(IEnumerable<EffectBase> effects)
        {
            var writer = new BigEndianWriter();

            foreach (var effect in effects)
                effect.Serialize(writer);

            writer.Dispose();

            return writer.Buffer;
        }

        public static List<EffectBase> DeserializeEffects(byte[] buffer)
        {
            List<EffectBase> result = new();
            var reader = new BigEndianReader(buffer);

            while (reader.BytesAvailable is not 0)
                result.Add(EffectBase.Deserialize(reader));

            reader.Dispose();

            return result;
        }

        public static byte[] SerializeSetEffects(IEnumerable<IEnumerable<EffectBase>> setEffects)
        {
            var writer = new BigEndianWriter();

            foreach (var setEffect in setEffects)
            {
                var effectsSerialized = SerializeEffects(setEffect);
                writer.WriteInt(effectsSerialized.Length);
                writer.WriteBytes(effectsSerialized);
            }

            writer.Dispose();

            return writer.Buffer;
        }

        public static List<List<EffectBase>> DeserializeSetEffects(byte[] buffer)
        {
            List<List<EffectBase>> result = new();
            var reader = new BigEndianReader(buffer);

            while (reader.BytesAvailable is not 0)
                result.Add(DeserializeEffects(reader.ReadBytes(reader.ReadInt())));

            return result;
        }

        public EffectRecord? GetEffectRecordById(int id) =>
            _records.ContainsKey(id) ? _records[id] : default;
    }
}
