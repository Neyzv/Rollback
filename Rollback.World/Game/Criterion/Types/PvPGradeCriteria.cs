using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.Game.Criterion.Enums;

namespace Rollback.World.Game.Criterion.Types
{
    [Identifier("PP"), Identifier("Pp")]
    public sealed class PvPGradeCriteria : BaseCriteria
    {
        public PvPGradeCriteria(string identifier, Comparator comparator, string value, Operator op) : base(identifier, comparator, value, op) { }
    }
}
