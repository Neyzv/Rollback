#nullable disable

using System.Net;
using System.Text.Json.Serialization;
using Rollback.Common.Config;
using Rollback.Common.DesignPattern.Instance;

namespace Rollback.World.Network.IPC.Config
{
    [Config("IPC")]
    public sealed class IPCServiceConfiguration : Singleton<IPCServiceConfiguration>
    {
        public short WorldId { get; set; } = 30;

        public ushort ServerCapacity { get; set; } = 2000;

        public string Hostname { get; set; } = "127.0.0.1";

        public int Port { get; set; } = 9000;

        public bool DebugMod { get; set; } = false;

        [JsonIgnore]
        public IPEndPoint IPEndPoint =>
            new(IPAddress.Parse(Hostname), Port);
    }
}
