using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.Game.Criterion.Enums;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Criterion.Types
{
    [Identifier("BI")]
    public sealed class UnusableCriteria : BaseCriteria
    {
        public UnusableCriteria(string identifier, Comparator comparator, string value, Operator op) : base(identifier, comparator, value, op) { }

        public override bool Eval(Character character) =>
            false;
    }
}
