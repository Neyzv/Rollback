using System.Net;
using System.Text.Json.Serialization;

namespace Rollback.Common.Network.Config
{
    public interface INetworkConfig
    {
        public string Hostname { get; set; }

        public ushort Port { get; set; }

        public int MaxConcurrentConnections { get; set; }

        public bool DebugMod { get; set; }

        [JsonIgnore]
        public IPEndPoint IPEndPoint =>
            new(IPAddress.Parse(Hostname), Port);
    }
}
