using Rollback.Auth.Managers;
using Rollback.Auth.Network;
using Rollback.Auth.Network.Handler;
using Rollback.Auth.Network.IPC;
using Rollback.Auth.Network.IPC.Handlers.Connection;
using Rollback.Auth.Objects;
using Rollback.Common.Extensions;
using Rollback.Protocol.Enums;
using Rollback.Protocol.Messages;

namespace Rollback.Auth.Handlers
{
    public static class ConnectionHandler
    {
        private static bool Identification(AuthClient client, IdentificationMessage message)
        {
            bool res = false;

            if (message.version.Major != GeneralConfiguration.Instance.Version.Major ||
                message.version.Minor != GeneralConfiguration.Instance.Version.Minor ||
                message.version.Revision != GeneralConfiguration.Instance.Version.Revision ||
                message.version.BuildType != GeneralConfiguration.Instance.Version.BuildType)
                SendIdentificationFailedForBadVersionMessage(client);
            else
            {
                var account = AccountManager.GetAccountByLogin(message.login);
                if (account is null || client.Ticket is null || StringExtensions.CipherPassword(account.Password, client.Ticket) != message.password)
                    SendIdentificationFailedMessage(client, IdentificationFailureReasonEnum.WRONG_CREDENTIALS);
                else if (account.IsBanned)
                    SendIdentificationFailedMessage(client, IdentificationFailureReasonEnum.BANNED);
                else
                {
                    var connected = AuthServer.Instance.GetClient(x => x.Account?.Id == account.Id);
                    connected?.Dispose();

                    client.Account = account;
                    client.Account.Ticket = client.Ticket;
                    client.Account.LastIP = client.IP;
                    client.Account.LastConnection = DateTime.Now;

                    if (account.Nickname == string.Empty)
                        SendNicknameRegistrationMessage(client);
                    else
                    {
                        foreach (var world in IPCServer.Instance.GetClients()) // disconnect client if he is connected to a server
                            IPCConnectionHandler.SendAccountConnectedMessage(world, account.Id);

                        SendIdentificationSuccessMessage(client);

                        res = true;
                    }
                }
            }

            return res;
        }

        [AuthHandler(IdentificationMessage.Id, false)]
        public static void HandleIdentificationMessage(AuthClient client, IdentificationMessage message)
        {
            if (Identification(client, message))
                SendServersListMessage(client);
        }

        [AuthHandler(IdentificationMessageWithServerIdMessage.Id, false)]
        public static void HandleIdentificationMessageWithServerIdMessage(AuthClient client, IdentificationMessageWithServerIdMessage message)
        {
            if (Identification(client, message))
                HandleServerSelectionMessage(client, new(message.serverId));
        }

        [AuthHandler(NicknameChoiceRequestMessage.Id, false)]
        public static void HandleNicknameChoiceRequestMessage(AuthClient client, NicknameChoiceRequestMessage message)
        {
            if (client.Account is not null)
            {
                if (message.nickname == client.Account.Login)
                    SendNicknameRefusedMessage(client, NicknameErrorEnum.SAME_AS_LOGIN);
                else if (message.nickname.ToLower().Contains(client.Account.Login.ToLower()))
                    SendNicknameRefusedMessage(client, NicknameErrorEnum.TOO_SIMILAR_TO_LOGIN);
                else if (!AccountManager.IsNicknameCorrect(message.nickname))
                    SendNicknameRefusedMessage(client, NicknameErrorEnum.INVALID_NICK);
                else if (AccountManager.GetAccountByNickname(message.nickname) is not null)
                    SendNicknameRefusedMessage(client, NicknameErrorEnum.ALREADY_USED);
                else
                {
                    client.Account.Nickname = message.nickname;
                    client.Save();

                    SendNicknameAcceptedMessage(client);
                }
            }
        }

        [AuthHandler(ServerSelectionMessage.Id)]
        public static void HandleServerSelectionMessage(AuthClient client, ServerSelectionMessage message)
        {
            var server = IPCServer.Instance.GetClient(x => x.World?.Record.Id == message.serverId);
            if (server is not null && client.Account is not null && server.World!.CanAccess(client.Account))
                QueueManager.Instance.Enqueue(client, server);
        }

        public static void SendQueueStatusMessage(AuthClient client, ushort position, ushort total) =>
            client.Send(new QueueStatusMessage(position, total));

        public static void SendLoginQueueStatusMessage(AuthClient receiver, ushort position, ushort total) =>
            receiver.Send(new LoginQueueStatusMessage(position, total));

        public static void SendProtocolRequired(AuthClient receiver) =>
            receiver.Send(new ProtocolRequired(1165, 1165));

        public static void SendHelloConnectMessage(AuthClient client)
        {
            do
            {
                client.Ticket = Random.Shared.RandomString(32);
            }
            while (AuthServer.Instance.GetClient(x => x != client && x.Ticket == client.Ticket) is not null || AccountManager.GetAccountByTicket(client.Ticket) is not null);

            client.Send(new HelloConnectMessage(client.Ticket));
        }

        public static void SendIdentificationFailedForBadVersionMessage(AuthClient client) =>
            client.Send(new IdentificationFailedForBadVersionMessage((sbyte)IdentificationFailureReasonEnum.BAD_VERSION, GeneralConfiguration.Instance.Version));

        public static void SendIdentificationFailedMessage(AuthClient client, IdentificationFailureReasonEnum reason) =>
            client.Send(new IdentificationFailedMessage((sbyte)reason));

        public static void SendNicknameRegistrationMessage(AuthClient client) =>
            client.Send(new NicknameRegistrationMessage());

        public static void SendNicknameRefusedMessage(AuthClient client, NicknameErrorEnum reason) =>
            client.Send(new NicknameRefusedMessage((sbyte)reason));

        public static void SendNicknameAcceptedMessage(AuthClient client) =>
            client.Send(new NicknameAcceptedMessage());

        public static void SendIdentificationSuccessMessage(AuthClient client) => // TO DO community
            client.Send(new IdentificationSuccessMessage(client.Account!.IsAdmin, false, client.Account!.Nickname, 0, client.Account.SecretQuestion, 42195168000000));

        public static void SendServersListMessage(AuthClient client) =>
            client.Send(new ServersListMessage(WorldManager.Instance.GetWorlds().Select(x => x.GetGameServerInformations(client.Account!)).ToArray()));

        public static void SendServerStatusUpdateMessage(AuthClient client, World world) =>
            client.Send(new ServerStatusUpdateMessage(world.GetGameServerInformations(client.Account!)));

        public static void SendSelectedServerDataMessage(AuthClient client, IPCReceiver server) =>
            client.Send(new SelectedServerDataMessage((short)server.World!.Record.Id, server.World.IP!, server.World.Port!.Value, client.Account!.CharactersByWorld!.SelectMany(x => x.Value).Count() < GeneralConfiguration.Instance.MaxCharactersByAccount, client.Account!.Ticket));
    }
}
