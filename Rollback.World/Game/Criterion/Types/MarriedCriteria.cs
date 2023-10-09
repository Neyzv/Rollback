using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.Game.Criterion.Enums;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Criterion.Types
{
    [Identifier("PR")]
    internal class MarriedCriteria : BaseCriteria
    {
        public MarriedCriteria(string identifier, Comparator comparator, string value, Operator op) : base(identifier, comparator, value, op) { }

        public override bool Eval(Character character) =>
            Value switch
            {
                "0" => character.Spouse is null,
                _ => character.Spouse is not null
            };
    }
}
