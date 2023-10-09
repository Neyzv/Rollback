using Rollback.Common.DesignPattern.Attributes;
using Rollback.Protocol.Enums;
using Rollback.World.CustomEnums;

namespace Rollback.World.Game.Stats.Fields
{
    [Identifier(Stat.Chance)]
    public sealed class ChanceField : BasicStatField
    {
        public ChanceField(StatsData stats) : base(stats) { }

        protected override void Update(short delta)
        {
            short baseProspection = (short)(_stats.Owner is { Breed: BreedEnum.Enutrof } ? 120 : 100);
            _stats[Stat.Prospecting].Base = (short)(baseProspection + (_stats[Stat.Chance].Total / 10));

            base.Update(delta);
        }
    }
}
