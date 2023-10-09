using System.Net.Sockets;
using Rollback.Common.Config;
using Rollback.Common.DesignPattern.Instance;
using Rollback.Common.Initialization;
using Rollback.Common.IO.Binary;
using Rollback.Common.Logging;
using Rollback.Common.Network.IPC;
using Rollback.Common.Network.IPC.Handler;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Enums;
using Rollback.World.Network.IPC.Config;
using Rollback.World.Network.IPC.Handlers.Connection;

namespace Rollback.World.Network.IPC
{
    internal class IPCService : Singleton<IPCService>, IDisposable
    {
        private const int RequestLifeTime = 5000;

        private static readonly UniqueIdProvider _uniqueIdProvider;

        private readonly Socket _socket;
        private readonly IPCServiceConfiguration _config;
        private readonly CancellationTokenSource _cts;
        private IPCMessage? _lastReceivedMessage;
        private bool _isReady = false;
        private readonly Dictionary<int, Timer> _requestTimers;

        static IPCService() =>
            _uniqueIdProvider = new();

        public IPCService()
        {
            _socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _config = ConfigManager.Instance.Get<IPCServiceConfiguration>();
            _cts = new();
            _requestTimers = new();
        }

        [Initializable(InitializationPriority.IPCClient, "IPC")]
        public void Initialize()
        {
            Task.Run(Start);
            WorldServer.Instance.Ready += OnReady;
        }

        private void OnReady()
        {
            while (!_isReady) { } // wait until the IPC is ready

            if (!_cts.IsCancellationRequested)
                WorldServer.Instance.ChangeStatus(ServerStatusEnum.ONLINE);
        }

        public void Start()
        {
            while (!_cts.IsCancellationRequested && !_socket.Connected)
            {
                try
                {
                    _socket.Connect(_config.IPEndPoint);
                }
                catch
                {
                    Logger.Instance.LogInfo("Trying to connect to auth server...");
                    Thread.Sleep(1000);
                }
            }

            IPCConnectionHandler.SendWorldConnectedMessage(_config.WorldId, WorldServer.Instance.Config.Hostname, WorldServer.Instance.Config.Port);
            WorldServer.Instance.ChangeStatus(ServerStatusEnum.STARTING);

            _isReady = true;

            Listen();
        }

        private void Listen()
        {
            try
            {
                while (!_cts.IsCancellationRequested && _socket.Connected)
                {
                    Span<byte> buffer = new byte[9024];

                    var bufferSize = _socket.Receive(buffer, SocketFlags.None);
                    if (bufferSize < Message.headerSize)
                    {
                        Logger.Instance.LogWarn($"Force disconnection of client {this}, corrupted packet received...");
                        Dispose();
                    }

                    buffer = buffer[..bufferSize];
                    Handle(buffer);
                }
            }
            catch (Exception e)
            {
                Logger.Instance.LogWarn($"Force disconnection of client {this} : {e.Message}", e);
                Dispose();
            }
        }

        private static bool GetMessage(int msgId, out IPCMessage? message) =>
            ProtocolManager.Instance.TryGetIPCMessage(msgId, out message);

        private void ExecuteHandler(IPCMessage message)
        {
            var (handler, attribute) = IPCHandlerManager.Instance.GetHandler(message.MessageId);
            if (handler is not null && attribute is not null)
            {
                handler.DynamicInvoke(message);

                if (_config.DebugMod)
                    Logger.Instance.LogInfo($"IPCMessage {message.GetType().Name} handled successfully !");
            }
        }

        private void Handle(Span<byte> buffer)
        {
            try
            {
                using var reader = new BigEndianReader(buffer.ToArray());
                while (!_cts.IsCancellationRequested && _socket.Connected && reader.BytesAvailable >= Message.headerSize)
                {
                    var hiHeader = reader.ReadShort();
                    var msgId = hiHeader >> Message.headerSize;

                    if (!GetMessage(msgId, out var message))
                        throw new Exception($"Can not find message {msgId}...");

                    if (message!.UnPack(reader, (byte)(hiHeader & Message.bitShift)))
                        ExecuteHandler(message);

                    _lastReceivedMessage = message;
                }
            }
            catch (Exception e)
            {
                Logger.Instance.LogWarn($"Force disconnection of client {this} : {e.Message}", e);
                Dispose();
            }
        }

        public void Send(IPCMessage message)
        {
            if (!_cts.IsCancellationRequested && _socket.Connected)
            {
                using var writer = new BigEndianWriter();
                message.Pack(writer);
                _socket.Send(writer.Buffer);
            }
            else
                Dispose();
        }

        private void TimerElapsed(object? state)
        {
            Logger.Instance.LogInfo("IPC Server take too much time to response, connection aborted...");
            Dispose();
        }

        public void SendRequest<TResponse, TError>(IPCMessage requestMessage, Action<TResponse> responseAction, Action<TError>? errorAction = default)
            where TResponse : IPCMessage
            where TError : IPCMessage
        {
            var response = _lastReceivedMessage;

            requestMessage.RequestId = _uniqueIdProvider.Generate();

            _requestTimers.Add(requestMessage.RequestId, new Timer(new(TimerElapsed), default, RequestLifeTime, Timeout.Infinite));

            Send(requestMessage);

            while (response is not TResponse && response is not TError || response.RequestId != requestMessage.RequestId)
                response = _lastReceivedMessage;

            lock (_lastReceivedMessage!)
                if (_lastReceivedMessage.RequestId == requestMessage.RequestId)
                    _lastReceivedMessage = default;

            if (_requestTimers.TryGetValue(requestMessage.RequestId, out var timer))
            {
                timer.Dispose();
                _requestTimers.Remove(requestMessage.RequestId);
            }

            _uniqueIdProvider.Free(requestMessage.RequestId);

            if (response is TResponse answ)
                responseAction(answ);
            else if (errorAction is not null && response is TError error)
                errorAction(error);
        }

        public void Dispose()
        {
            if (!_cts.IsCancellationRequested)
            {
                _cts.Cancel();
                _cts.Dispose();
            }

            _socket?.Dispose();

            WorldServer.Instance?.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}
