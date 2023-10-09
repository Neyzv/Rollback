using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.Game.Criterion.Enums;

namespace Rollback.World.Game.Criterion.Types
{
    [Identifier("Pg")]
    public sealed class GiftCriteria : BaseCriteria // Don
    {
        public GiftCriteria(string identifier, Comparator comparator, string value, Operator op) : base(identifier, comparator, value, op)
        {
        }
    }
}
