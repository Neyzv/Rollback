using System.Net;
using System.Net.Sockets;
using Rollback.Common.Logging;

namespace Rollback.Common.Network
{
    public abstract class Client<TMessage> : IDisposable
        where TMessage : IMessage
    {
        protected readonly Socket _socket;
        protected readonly CancellationToken _ct;

        private string? _ip;
        public string? IP =>
            _ip ??= (_socket.RemoteEndPoint as IPEndPoint)?.Address.ToString();

        public event Action? Disconnected;
        public event Action<TMessage>? MessageSend;

        protected Client(Socket socket, CancellationToken ct) =>
           (_socket, _ct) = (socket, ct);

        protected abstract byte[] PackMessage(TMessage message);

        public void Send(TMessage message)
        {
            try
            {
                if (!_ct.IsCancellationRequested && _socket.Connected)
                {
                    _socket.Send(PackMessage(message));
                    MessageSend?.Invoke(message);
                }
                else
                    Dispose();
            }
            catch(SocketException _) {}
            catch (Exception e)
            {
                Logger.Instance.LogWarn($"Force disconnection of client {this} : {e.Message}", e);
                Dispose();
            }
        }

        public abstract void Listen();

        protected abstract void ExecuteHandler(TMessage message);

        protected abstract void Handle(Span<byte> buffer);

        public abstract void Save();

        public void Dispose()
        {
            Disconnected?.Invoke();

            Save();

            _socket.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
