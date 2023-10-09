using System.Net.Sockets;
using Rollback.Common.IO.Binary;
using Rollback.Common.Logging;
using Rollback.Common.Network.Protocol;

namespace Rollback.Common.Network
{
    public abstract class DofusClient<TMessage> : Client<TMessage>
        where TMessage : Message
    {
        public event Action<Message>? MessageReceived;

        protected DofusClient(Socket socket, CancellationToken ct) : base(socket, ct) { }

        protected override byte[] PackMessage(TMessage message)
        {
            using var writer = new BigEndianWriter();
            message.Pack(writer);

            return writer.Buffer;
        }

        public override void Listen()
        {
            try
            {
                while (!_ct.IsCancellationRequested && _socket.Connected)
                {
                    Span<byte> buffer = new byte[9024];

                    var bufferSize = -1;
                    try
                    {
                        bufferSize = _socket.Receive(buffer, SocketFlags.None);
                    }
                    catch
                    {
                        Dispose();
                    }

                    if (bufferSize is not -1)
                    {
                        if (bufferSize < Message.headerSize)
                        {
                            if (bufferSize is not 0)
                                Logger.Instance.LogWarn($"Force disconnection of client {this}, corrupted packet received...");
                            Dispose();
                        }
                        else
                        {
                            buffer = buffer[..bufferSize];
                            Handle(buffer);
                        }
                    }

                }
            }
            catch (Exception e)
            {
                Logger.Instance.LogWarn($"Force disconnection of client {this} : {e.Message}", e);
                Dispose();
            }
        }

        protected abstract bool GetMessage(int msgId, out TMessage? message);

        protected override void Handle(Span<byte> buffer)
        {
            try
            {
                using var reader = new BigEndianReader(buffer.ToArray());
                while (!_ct.IsCancellationRequested && _socket.Connected && reader.BytesAvailable >= Message.headerSize)
                {
                    var hiHeader = reader.ReadShort();
                    var msgId = hiHeader >> Message.headerSize;

                    if (!GetMessage(msgId, out var message))
                        throw new Exception($"Can not find message {msgId}...");

                    if (message!.UnPack(reader, (byte)(hiHeader & Message.bitShift)))
                    {
                        MessageReceived?.Invoke(message);
                        ExecuteHandler(message);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Instance.LogWarn($"Force disconnection of client {this} : {e.Message}", e);
                Dispose();
            }
        }
    }
}
