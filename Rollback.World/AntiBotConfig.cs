using Rollback.Common.Config;
using Rollback.Common.DesignPattern.Instance;

namespace Rollback.World
{
    [Config("AnitBot")]
    public sealed class AntiBotConfig : Singleton<AntiBotConfig>
    {
        public byte TimeToTravelRun { get; set; } = 140;

        public short TimeToTravelWalk { get; set; } = 380;
    }
}
