using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;

namespace Rollback.World.Game.Stats.Fields
{
    [Identifier(Stat.Initiative)]
    public sealed class InitiativeField : StatField
    {
        public override short TotalWithOutContext =>
        (short)(base.TotalWithOutContext * (_stats.Health.Actual / (double)_stats.Health.ActualMax));

        public InitiativeField(StatsData stats) : base(stats) { }
    }
}
