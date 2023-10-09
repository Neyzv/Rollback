using Rollback.Common.DesignPattern.Attributes;

namespace Rollback.World.Network.Handler
{
    internal class WorldHandlerAttribute : HandlerAttribute
    {
        public bool Connected { get; set; }

        public WorldHandlerAttribute(uint messageId, bool connected = true) : base(messageId) =>
            Connected = connected;
    }
}
