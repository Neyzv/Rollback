using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.Game.Criterion.Enums;

namespace Rollback.World.Game.Criterion.Types
{
    [Identifier("Ps")]
    public sealed class AlignmentCriteria : BaseCriteria
    {
        public AlignmentCriteria(string identifier, Comparator comparator, string value, Operator op) : base(identifier, comparator, value, op) { }
    }
}
