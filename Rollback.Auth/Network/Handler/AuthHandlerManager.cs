using Rollback.Common.Initialization;
using Rollback.Common.Network.Handler;

namespace Rollback.Auth.Network.Handler
{
    internal class AuthHandlerManager : HandlerManager<AuthHandlerManager, AuthHandlerAttribute>
    {
        [Initializable(InitializationPriority.Database, "Dofus Handlers")]
        public void Init() =>
            Initialize();

        public override (Delegate, AuthHandlerAttribute) GetHandler(uint messId) =>
            _handlers.ContainsKey(messId) ? _handlers[messId] : default;
    }
}
