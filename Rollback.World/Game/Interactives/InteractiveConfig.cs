using Rollback.Common.Config;
using Rollback.Common.DesignPattern.Instance;

namespace Rollback.World.Game.Interactives
{
    [Config("Interactives")]
    public sealed class InteractiveConfig : Singleton<InteractiveConfig>
    {
        public int RegrowMinTime { get; set; } = 60_000;

        public int RegrowMaxTime { get; set; } = 120_000;
    }
}
