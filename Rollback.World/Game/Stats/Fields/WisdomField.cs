using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;

namespace Rollback.World.Game.Stats.Fields
{
    [Identifier(Stat.Wisdom)]
    public sealed class WisdomField : StatField
    {
        public WisdomField(StatsData stats) : base(stats) { }

        protected override void Update(short delta)
        {
            _stats[Stat.DodgeApLostProbability].Additional = (short)Math.Floor(_stats[Stat.DodgeApLostProbability].Base + Total / 10d);
            _stats[Stat.DodgeMpLostProbability].Additional = (short)Math.Floor(_stats[Stat.DodgeMpLostProbability].Base + Total / 10d);
        }
    }
}
