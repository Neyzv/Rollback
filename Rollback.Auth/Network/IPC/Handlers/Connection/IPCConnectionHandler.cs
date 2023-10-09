using Rollback.Auth.Database;
using Rollback.Auth.Managers;
using Rollback.Common.Extensions;
using Rollback.Common.Logging;
using Rollback.Common.Network.IPC.Handler;
using Rollback.Common.Network.IPC.Messages.Connection;
using Rollback.Protocol.Enums;

namespace Rollback.Auth.Network.IPC.Handlers.Connection
{
    public static class IPCConnectionHandler
    {
        [IPCHandler(WorldConnectedMessage.Id, false)]
        public static void HandleWorldConnectedMessage(IPCReceiver client, WorldConnectedMessage message)
        {
            if (client.World is null && IPCServer.Instance.GetClient(x => x.World?.Record.Id == message.WorldId) is null)
            {
                var world = WorldManager.Instance.GetWorldById(message.WorldId);
                if (world is not null)
                {
                    world.IP = message.IPAddress;
                    world.Port = message.Port;
                    client.World = world;
                    world.ChangeState(ServerStatusEnum.OFFLINE);

                    Logger.Instance.LogInfo($"World {message.WorldId} connected !");
                }
                else
                    Logger.Instance.LogWarn($"World {message.WorldId} doesn't exist...");
            }
            else
                Logger.Instance.LogWarn($"World {message.WorldId} is already connected...");
        }

        [IPCHandler(ClientConnectionRequestMessage.Id)]
        public static void HandleClientConnectionRequestMessage(IPCReceiver client, ClientConnectionRequestMessage message)
        {
            var account = AccountManager.GetAccountByTicket(message.Ticket!);

            if (account is null)
                SendClientConnectionRefusedMessage(client);
            else
                SendClientConnectionAcceptedMessage(client, account);
        }

        public static void SendClientConnectionAcceptedMessage(IPCReceiver client, AccountRecord account) =>
            client.Send(new ClientConnectionAcceptedMessage(account.Id, account.Nickname, (sbyte)account.Role,
                account.CharactersByWorld!.ContainsKey(client.World!.Record.Id) ? account.CharactersByWorld[client.World!.Record.Id].ToArray() : Array.Empty<int>(),
                account.SecretAnswer, (sbyte)(GeneralConfiguration.Instance.MaxCharactersByAccount - account.CharactersByWorld!.SelectMany(x => x.Value).Count()),
                account.LastConnection is null ? -1 : account.LastConnection.Value.GetUnixTimeStamp(), account.LastIP!,
                account.Gifts.Where(x => !x.UnavailableServerIds.Contains(client.World.Record.Id)).Select(x => x.GiftInformations).ToArray()));

        public static void SendClientConnectionRefusedMessage(IPCReceiver client) =>
            client.Send(new ClientConnectionRefusedMessage());

        public static void SendAccountConnectedMessage(IPCReceiver client, int accountId) =>
            client.Send(new AccountConnectedMessage(accountId));
    }
}
