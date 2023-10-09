using Rollback.Common.Network.IPC.Handler;
using Rollback.Common.Network.IPC.Messages.World;
using Rollback.Protocol.Enums;

namespace Rollback.Auth.Network.IPC.Handlers.World
{
    public static class IPCWorldHandler
    {
        [IPCHandler(WorldStateUpdateMessage.Id)]
        public static void HandleWorldStateUpdateMessage(IPCReceiver client, WorldStateUpdateMessage message)
        {
            if ((sbyte)client.World!.State != message.State && Enum.IsDefined(typeof(ServerStatusEnum), (int)message.State))
                client.World.ChangeState((ServerStatusEnum)message.State);
        }
    }
}
