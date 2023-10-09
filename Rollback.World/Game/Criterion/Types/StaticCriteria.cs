using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.Game.Criterion.Enums;

namespace Rollback.World.Game.Criterion.Types
{
    [Identifier("Sc")]
    public sealed class StaticCriteria : BaseCriteria
    {
        public StaticCriteria(string identifier, Comparator comparator, string value, Operator op) : base(identifier, comparator, value, op) { }
    }
}
