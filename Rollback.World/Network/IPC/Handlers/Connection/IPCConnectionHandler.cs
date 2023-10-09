using Rollback.Common.Network.IPC.Handler;
using Rollback.Common.Network.IPC.Messages.Connection;

namespace Rollback.World.Network.IPC.Handlers.Connection
{
    public static class IPCConnectionHandler
    {
        [IPCHandler(AccountConnectedMessage.Id)]
        public static void HandleAccountConnectedMessage(AccountConnectedMessage message) =>
            WorldServer.Instance.GetClient(x => x.Account?.Id == message.AccountId)?.Dispose();

        public static void SendWorldConnectedMessage(short worldId, string ip, ushort port) =>
            IPCService.Instance.Send(new WorldConnectedMessage(worldId, ip, port));
    }
}
