using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.Game.Criterion.Enums;

namespace Rollback.World.Game.Criterion.Types
{
    [Identifier("Pa")]
    public sealed class AlignmentLevelCriteria : BaseCriteria
    {
        public AlignmentLevelCriteria(string identifier, Comparator comparator, string value, Operator op) : base(identifier, comparator, value, op)
        {
        }
    }
}
