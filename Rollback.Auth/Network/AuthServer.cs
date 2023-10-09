using System.Net.Sockets;
using Rollback.Auth.Managers;
using Rollback.Common.Initialization;
using Rollback.Common.Network;
using Rollback.Common.Network.Config;
using Rollback.Common.Network.Protocol;

namespace Rollback.Auth.Network
{
    public sealed class AuthServer : Server<AuthServer, AuthClient, Message, NetworkConfig>
    {
        [Initializable(InitializationPriority.Network, "Network")]
        public void Initialize() =>
            Start();

        public AuthServer() : base() =>
            Stop += () =>
            {
                if (Config.DebugMod)
                {
                    Console.WriteLine("Press any key to exit...");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            };

        protected override AuthClient CreateClient(Socket socket)
        {
            var authClient = new AuthClient(socket, CancellationToken);

            QueueManager.Instance.Enqueue(authClient);

            return authClient;
        }

        protected override bool CanAddClient(AuthClient client) =>
            GetClients(x => x.IP == client.IP).Length < Config.MaxConnectionsPerIP;
    }
}
