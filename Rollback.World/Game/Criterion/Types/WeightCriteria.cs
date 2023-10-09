using Rollback.Common.DesignPattern.Attributes;
using Rollback.Common.Extensions;
using Rollback.World.Game.Criterion.Enums;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Criterion.Types
{
    [Identifier("PW")]
    public sealed class WeightCriteria : BaseCriteria
    {
        private int? _pod;
        public int Pod =>
            _pod ??= Value.ChangeType<int>();

        public WeightCriteria(string identifier, Comparator comparator, string value, Operator op) : base(identifier, comparator, value, op) { }

        public override bool Eval(Character character) =>
            Compare(character.Inventory.Pods, Pod);
    }
}
