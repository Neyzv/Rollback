using System.Net.Sockets;
using Rollback.Auth.Objects;
using Rollback.Common.Logging;
using Rollback.Common.Network;
using Rollback.Common.Network.IPC;
using Rollback.Common.Network.IPC.Handler;
using Rollback.Common.Network.Protocol;

namespace Rollback.Auth.Network.IPC
{
    public sealed class IPCReceiver : DofusClient<IPCMessage>
    {
        private int _lastRequestId;

        public World? World { get; set; }

        public IPCReceiver(Socket socket, CancellationToken ct) : base(socket, ct) { }

        protected override bool GetMessage(int msgId, out IPCMessage? message) =>
            ProtocolManager.Instance.TryGetIPCMessage(msgId, out message);

        protected override byte[] PackMessage(IPCMessage message)
        {
            message.RequestId = _lastRequestId;

            return base.PackMessage(message);
        }

        protected override void ExecuteHandler(IPCMessage message)
        {
            var (handler, attribute) = IPCHandlerManager.Instance.GetHandler(message.MessageId);
            if (handler is not null && attribute is not null)
            {
                if (attribute.Connected || World is null)
                {
                    _lastRequestId = message.RequestId;

                    handler.DynamicInvoke(this, message);

                    if (IPCServer.Instance.Config.DebugMod)
                        Logger.Instance.LogInfo($"IPCMessage {message.GetType().Name} handled successfully !");
                }
                else
                {
                    Dispose();
                    Logger.Instance.LogWarn($"Force disconnection of client : IPCReceiver must have a world linked to handle message {message.GetType().Name}...");
                }
            }
            else
                Logger.Instance.LogWarn($"Can not find a handler for message {message.GetType().Name}...");
        }

        public override void Save() =>
            World?.ChangeState(Protocol.Enums.ServerStatusEnum.STATUS_UNKNOWN);

        public override string ToString() =>
            $"World {(World is null ? IP : World.Record.Id)}";
    }
}
