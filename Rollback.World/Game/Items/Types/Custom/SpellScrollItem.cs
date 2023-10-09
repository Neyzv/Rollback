using Rollback.World.CustomEnums;
using Rollback.World.Database.Characters;
using Rollback.World.Game.Effects.Types;
using Rollback.World.Game.Items.Attributes;
using Rollback.World.Game.Items.Storages;
using Rollback.World.Game.World.Maps;

namespace Rollback.World.Game.Items.Types.Custom
{
    [ItemType((short)ItemType.ParcheminDeSort)]
    public sealed class SpellScrollItem : PlayerItem
    {
        public SpellScrollItem(CharacterItemRecord record, Inventory inventory)
            : base(record, inventory) { }

        public override bool Use(Cell? targetedCell) =>
            Effects.FirstOrDefault(x => x.Id is EffectId.EffectLearnSpell) is EffectInteger effectInteger &&
            _storage.Owner.GetSpell(effectInteger.Value) is null;
    }
}
