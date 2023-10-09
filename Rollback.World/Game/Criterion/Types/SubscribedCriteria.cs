using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.Game.Criterion.Enums;

namespace Rollback.World.Game.Criterion.Types
{
    [Identifier("PZ")]
    public sealed class SubscribedCriteria : BaseCriteria
    {
        public SubscribedCriteria(string identifier, Comparator comparator, string value, Operator op) : base(identifier, comparator, value, op)
        {
        }
    }
}
