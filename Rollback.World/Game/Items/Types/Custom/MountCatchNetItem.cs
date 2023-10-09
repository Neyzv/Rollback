using Rollback.World.CustomEnums;
using Rollback.World.Database.Characters;
using Rollback.World.Game.Effects.Types;
using Rollback.World.Game.Fights;
using Rollback.World.Game.Fights.Buffs.Types;
using Rollback.World.Game.Fights.Fighters;
using Rollback.World.Game.Items.Attributes;
using Rollback.World.Game.Items.Storages;
using Rollback.World.Game.Mounts;

namespace Rollback.World.Game.Items.Types.Custom;

[ItemId(597)]
public sealed class MountCatchNetItem : PlayerItem
{
    private const byte MountCatchPercent = 10;

    private EffectDice? _catchPercentageEffect;

    private EffectDice? CatchPercentageEffect =>
        _catchPercentageEffect ??= Effects.OfType<EffectDice>()
            .FirstOrDefault(x => x.Id is EffectId.EffectCatchMount);
    
    private static readonly IReadOnlyDictionary<short, short> _monsterIdToMountId = new Dictionary<short, short>()
    {
        [171] = 1,
        [200] = 6,
        [666] = 74
    };
    
    public MountCatchNetItem(CharacterItemRecord record, Inventory inventory)
        : base(record, inventory)
    {
        Created += OnItemCreated;
        Equipped += OnItemEquipped;
        Unequipped += OnItemUnequipped;
    }

    private void OnItemCreated(PlayerItem obj)
    {
        Effects.Clear();

        _catchPercentageEffect = new EffectDice()
        {
            Id = EffectId.EffectCatchMount,
            Value = MountCatchPercent
        };
        Effects.Add(_catchPercentageEffect);
    }

    private void OnWinnersDeterminated(IFight fight)
    {
        if (fight.Winners.Any(x => x.Id == _storage.Owner.Id && x.GetBuff<StateBuff>(stateBuff => stateBuff.State is SpellState.Apprivoisement) is not null) &&
            CatchPercentageEffect is not null && Random.Shared.Next(101) <= CatchPercentageEffect.Value)
        {
            foreach(var monster in fight.Losers.OfType<MonsterFighter>())
                if(_monsterIdToMountId.TryGetValue(monster.MonsterId, out var mountId))
                    if (MountManager.Instance.GetMountRecordById(mountId) is { } mountTemplate)
                    {
                        _storage.Owner.Fighter!.Result.AddEarnedItem(mountTemplate.CertificateId, 1);
                        _storage.RemoveItem(this, Quantity);
                        
                        break;
                    }
        }

        fight.WinnersDeterminated -= OnWinnersDeterminated;
    }

    private void OnEnterFight(IFight fight) =>
        fight.WinnersDeterminated += OnWinnersDeterminated;

    private void OnItemEquipped(PlayerItem obj) =>
        _storage.Owner.EnterFight += OnEnterFight;

    private void OnItemUnequipped(PlayerItem obj) =>
        _storage.Owner.FightEnded -= OnEnterFight;
}