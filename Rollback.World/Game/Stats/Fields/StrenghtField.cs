using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Jobs;

namespace Rollback.World.Game.Stats.Fields
{
    [Identifier(Stat.Strength)]
    public sealed class StrenghtField : BasicStatField
    {
        private const short BasePods = 1000;

        public StrenghtField(StatsData stats) : base(stats) =>
            _stats[Stat.Weight].Base = (short)(BasePods + (stats.Owner != null ? stats.Owner.Jobs.Sum(x => x.Value.Level) * JobManager.PodsPerJobLevel : 0));

        protected override void Update(short delta)
        {
            _stats[Stat.Weight].Base += (short)(delta * 5);
            base.Update(delta);
        }
    }
}
