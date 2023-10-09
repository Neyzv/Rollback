using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.Game.Criterion.Enums;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Criterion.Types
{
    [Identifier("POQ")]
    public sealed class HaveManyItemsCriteria : BaseCriteria
    {
        public HaveManyItemsCriteria(string identifier, Comparator comparator, string value, Operator op) : base(identifier, comparator, value, op) { }

        public override bool Eval(Character character)
        {
            var splittedValue = Value.Split(',');

            return splittedValue.Length > 1 && short.TryParse(splittedValue[0], out var itemId) && int.TryParse(splittedValue[1], out var quantity) &&
                character.Inventory.HaveItem(itemId, quantity) == (Comparator is Comparator.Equal);
        }
    }
}
