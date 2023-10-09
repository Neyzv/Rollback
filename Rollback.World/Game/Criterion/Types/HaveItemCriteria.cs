using Rollback.Common.DesignPattern.Attributes;
using Rollback.Common.Extensions;
using Rollback.World.Game.Criterion.Enums;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Criterion.Types
{
    [Identifier("PO")]
    public sealed class HaveItemCriteria : BaseCriteria
    {
        private short? _itemId;
        public short ItemId =>
            _itemId ??= Value.ChangeType<short>();

        public HaveItemCriteria(string identifier, Comparator comparator, string value, Operator op) : base(identifier, comparator, value, op) { }

        public override bool Eval(Character character) =>
            character.Inventory.HaveItem(ItemId) == (Comparator is Comparator.Equal);
    }
}
