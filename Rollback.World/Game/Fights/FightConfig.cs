using Rollback.Common.Config;
using Rollback.Common.DesignPattern.Instance;

namespace Rollback.World.Game.Fights
{
    [Config("Fights")]
    public sealed class FightConfig : Singleton<FightConfig>
    {
        public int PlacementPhaseTime { get; set; } = 30_000;

        public int PvTChallengersPlacementTime { get; set; } = 30_000;

        public int PvTDefendersPlacementTime { get; set; } = 10_000;

        public int TurnTime { get; set; } = 35_000;
    }
}
