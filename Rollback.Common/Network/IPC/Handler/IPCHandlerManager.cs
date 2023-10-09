using Rollback.Common.Initialization;
using Rollback.Common.Network.Handler;

namespace Rollback.Common.Network.IPC.Handler
{
    public class IPCHandlerManager : HandlerManager<IPCHandlerManager, IPCHandlerAttribute>
    {
        [Initializable(InitializationPriority.Database, "IPC Handlers")]
        public void Init() =>
            Initialize();

        public override (Delegate, IPCHandlerAttribute) GetHandler(uint messId) =>
            _handlers.ContainsKey(messId) ? _handlers[messId] : default;
    }
}
