using Rollback.Common.Config;
using Rollback.Common.Network.Config;

namespace Rollback.Auth.Network.IPC.Config
{
    [Config("IPC")]
    public sealed class IPCConfig : INetworkConfig
    {
        public string Hostname { get; set; } = "127.0.0.1";

        public ushort Port { get; set; }

        public int MaxConcurrentConnections { get; set; } = 100;

        public int MaxServers { get; set; } = 1;

        public bool DebugMod { get; set; } = default;
    }
}
