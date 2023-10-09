using System.Net.Sockets;
using Rollback.Common.Config;
using Rollback.Common.DesignPattern.Collections;
using Rollback.Common.DesignPattern.Instance;
using Rollback.Common.Logging;
using Rollback.Common.Network.Config;

namespace Rollback.Common.Network
{
    public abstract class Server<TInstance, TClient, TMessage, TConfig> : Singleton<TInstance>, IDisposable, IClientCollection<TClient, TMessage>
        where TInstance : Server<TInstance, TClient, TMessage, TConfig>
        where TClient : Client<TMessage>
        where TMessage : IMessage
        where TConfig : INetworkConfig
    {
        protected readonly SynchronizedCollection<TClient> _clients = new();

        private readonly CancellationTokenSource _cts;

        private readonly Socket _socket;

        protected event Action<TClient>? ClientConnected;
        protected event Action<TClient>? ClientDisconnected;
        public event Action? Ready;
        public event Action? Stop;

        protected CancellationToken CancellationToken =>
            _cts.Token;

        public TConfig Config
        {
            get;
        }

        public Server()
        {
            _socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _cts = GenerateServerToken();
            Config = ConfigManager.Instance.Get<TConfig>();

            ClientDisconnected += OnClientDisconnected;
            Logger.Instance.Error += () => Dispose();
        }

        private void OnClientDisconnected(TClient client)
        {
            _clients.Remove(client);
            Logger.Instance.LogInfo($"Client {client} disconnected...");
        }

        public void Start()
        {
            try
            {
                _socket.Bind(Config.IPEndPoint);
                _socket.Listen(Config.MaxConcurrentConnections);

                Logger.Instance.Log($"Server started on port {Config.Port}...");

                Ready?.Invoke();

                while (!_cts.IsCancellationRequested)
                {
                    var clientSocket = _socket.Accept();
                    if (!_cts.IsCancellationRequested)
                    {
                        var client = CreateClient(clientSocket);
                        client.Disconnected += () => ClientDisconnected?.Invoke(client);

                        if (!TryAddClient(client))
                        {
                            client.Dispose();
                            continue;
                        }

                        Logger.Instance.LogInfo($"Client {client} connected !");
                        ClientConnected?.Invoke(client);
                        Task.Run(client.Listen);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.LogError(ex);
            }
            finally
            {
                if (!_cts.IsCancellationRequested)
                    Dispose();
            }
        }

        protected abstract TClient CreateClient(Socket socket);

        protected abstract bool CanAddClient(TClient client);

        private bool TryAddClient(TClient client)
        {
            if (!CanAddClient(client))
                return false;

            _clients.Add(client);

            return true;
        }

        private CancellationTokenSource GenerateServerToken()
        {
            var token = new CancellationTokenSource();

            Console.CancelKeyPress += (sender, args) =>
            {
                args.Cancel = true;
                Dispose();
            };

            return token;
        }

        public TClient[] GetClients(Predicate<TClient>? p = default) =>
            (p is null ? _clients : _clients.Where(x => p(x))).ToArray();

        public TClient? GetClient(Predicate<TClient> p) =>
            _clients.FirstOrDefault(x => p(x));

        public void Send(Delegate d, object[]? parameters = null) =>
            IClientCollection<TClient, TMessage>.Send(GetClients(), d, parameters);

        public void Send(Predicate<TClient> p, Delegate d, object[]? parameters = null) =>
            IClientCollection<TClient, TMessage>.Send(GetClients(x => p(x)), d, parameters);

        public void Dispose()
        {
            Stop?.Invoke();

            if (!_cts.IsCancellationRequested)
            {
                _cts.Cancel();
                _cts.Dispose();
            }

            GC.SuppressFinalize(this);
        }
    }
}
