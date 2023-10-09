using System.Collections.Concurrent;
using Rollback.Auth.Handlers;
using Rollback.Auth.Network;
using Rollback.Auth.Network.IPC;
using Rollback.Common.DesignPattern.Instance;
using Rollback.Common.DesignPattern.Threading.Schedul;
using Rollback.Common.Initialization;

namespace Rollback.Auth.Managers
{
    internal sealed class QueueManager : Singleton<QueueManager>
    {
        private readonly ConcurrentQueue<AuthClient> _connectionQueue;
        private readonly ConcurrentQueue<(AuthClient, IPCReceiver)> _serverQueue;

        public QueueManager() =>
            (_connectionQueue, _serverQueue) = (new(), new());

        [Initializable(InitializationPriority.LowLevelDatasManager)]
        public void Initialize()
        {
            Scheduler.Instance.ExecutePeriodically(UpdateLogIn).WithTime(TimeSpan.FromSeconds(2));
            Scheduler.Instance.ExecutePeriodically(UpdateServerLogIn).WithTime(TimeSpan.FromSeconds(2));
        }

        public void Enqueue(AuthClient client) =>
            _connectionQueue.Enqueue(client);

        public void Enqueue(AuthClient client, IPCReceiver server) =>
            _serverQueue.Enqueue((client, server));

        private void UpdateLogIn()
        {
            while (_connectionQueue.TryDequeue(out var authClient))
            {
                if (!_connectionQueue.IsEmpty)
                {
                    ushort position = default;

                    foreach (var client in _connectionQueue)
                    {
                        position++;
                        ConnectionHandler.SendLoginQueueStatusMessage(client, position, (ushort)_connectionQueue.Count);
                    }
                }

                ConnectionHandler.SendProtocolRequired(authClient);
                ConnectionHandler.SendHelloConnectMessage(authClient);
            }
        }

        private void UpdateServerLogIn()
        {
            while (_serverQueue.TryDequeue(out var clientInfos))
            {
                ushort position = default;

                foreach (var client in _serverQueue)
                {
                    position++;
                    ConnectionHandler.SendQueueStatusMessage(client.Item1, position, (ushort)_serverQueue.Count);
                }

                ConnectionHandler.SendSelectedServerDataMessage(clientInfos.Item1, clientInfos.Item2);
                clientInfos.Item1.Dispose();
            }
        }
    }
}
