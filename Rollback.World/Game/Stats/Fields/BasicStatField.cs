using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;

namespace Rollback.World.Game.Stats.Fields
{
    [Identifier(Stat.Agility), Identifier(Stat.Intelligence)]
    public class BasicStatField : StatField
    {
        public BasicStatField(StatsData stats) : base(stats) { }

        protected override void Update(short delta) =>
            _stats[Stat.Initiative].Base += delta;
    }
}
