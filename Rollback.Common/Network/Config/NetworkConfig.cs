#nullable disable
using Rollback.Common.Config;

namespace Rollback.Common.Network.Config
{
    [Config("Network")]
    public sealed class NetworkConfig : INetworkConfig
    {
        public string Hostname { get; set; } = "127.0.0.1";

        public ushort Port { get; set; }

        public int MaxConcurrentConnections { get; set; } = 100;

        public int MaxConnectionsPerIP { get; set; } = 8;

        public bool DebugMod { get; set; } = default;
    }
}