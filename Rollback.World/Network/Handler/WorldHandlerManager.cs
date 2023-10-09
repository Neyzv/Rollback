using Rollback.Common.Initialization;
using Rollback.Common.Network.Handler;

namespace Rollback.World.Network.Handler
{
    internal class WorldHandlerManager : HandlerManager<WorldHandlerManager, WorldHandlerAttribute>
    {
        [Initializable(InitializationPriority.Database, "Dofus Handlers")]
        public void Init() =>
            Initialize();

        public override (Delegate, WorldHandlerAttribute) GetHandler(uint messId)
        {
            _handlers.TryGetValue(messId, out var result);

            return result;
        }
    }
}
