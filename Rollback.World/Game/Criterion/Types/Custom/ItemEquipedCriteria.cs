using Rollback.Common.DesignPattern.Attributes;
using Rollback.Common.Extensions;
using Rollback.Protocol.Enums;
using Rollback.World.Game.Criterion.Enums;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Criterion.Types.Custom
{
    [Identifier("POV")]
    public sealed class ItemEquipedCriteria : BaseCriteria
    {
        private short? _itemId;
        public short ItemId =>
            _itemId ??= Value.ChangeType<short>();

        public ItemEquipedCriteria(string identifier, Comparator comparator, string value, Operator op) : base(identifier, comparator, value, op) { }

        public override bool Eval(Character character)
        {
            var item = character.Inventory.GetItem(x => x.Id == ItemId && x.Position is not CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);

            return Comparator is Comparator.Equal ? item is not null : item is null;
        }
    }
}
