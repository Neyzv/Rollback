using System.Net.Sockets;
using Rollback.Auth.Network.IPC.Config;
using Rollback.Common.Initialization;
using Rollback.Common.Network;
using Rollback.Common.Network.IPC;

namespace Rollback.Auth.Network.IPC
{
    internal class IPCServer : Server<IPCServer, IPCReceiver, IPCMessage, IPCConfig>
    {
        [Initializable(InitializationPriority.IPCServer, "IPC")]
        public void Initialize() =>
            Task.Run(Start);

        public IPCServer() : base() { }

        protected override IPCReceiver CreateClient(Socket socket) =>
            new(socket, CancellationToken);

        protected override bool CanAddClient(IPCReceiver client) =>
            GetClients().Length < Config.MaxServers;
    }
}
