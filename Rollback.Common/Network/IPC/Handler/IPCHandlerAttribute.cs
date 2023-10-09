using Rollback.Common.DesignPattern.Attributes;

namespace Rollback.Common.Network.IPC.Handler
{
    public class IPCHandlerAttribute : HandlerAttribute
    {
        public bool Connected { get; set; }

        public IPCHandlerAttribute(uint messageId, bool connected = true) : base(messageId) =>
            Connected = connected;
    }
}
