using Rollback.Common.Network.IPC.Messages.World;
using Rollback.Protocol.Enums;

namespace Rollback.World.Network.IPC.Handlers.World
{
    public static class IPCWorldHandler
    {
        public static void SendWorldStateUpdateMessage(ServerStatusEnum state) =>
            IPCService.Instance.Send(new WorldStateUpdateMessage((sbyte)state));
    }
}
