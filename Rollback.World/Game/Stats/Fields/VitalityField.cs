using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;

namespace Rollback.World.Game.Stats.Fields
{
    [Identifier(Stat.Vitality)]
    public sealed class VitalityField : StatField
    {
        public VitalityField(StatsData stats) : base(stats)
        {
        }

        protected override void Update(short delta)
        {
            if (delta is not 0)
                _stats.Health.Actual += delta;
        }
    }
}
