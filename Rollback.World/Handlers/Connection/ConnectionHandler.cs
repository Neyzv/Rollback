using Rollback.Common.Extensions;
using Rollback.Common.Logging;
using Rollback.Common.Network.IPC.Messages.Connection;
using Rollback.Common.ORM;
using Rollback.Protocol.Enums;
using Rollback.Protocol.Messages;
using Rollback.World.Database.Accounts;
using Rollback.World.Database.Characters;
using Rollback.World.Database.Items;
using Rollback.World.Game.Accounts;
using Rollback.World.Game.Items;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Handlers.Basics;
using Rollback.World.Network;
using Rollback.World.Network.Handler;
using Rollback.World.Network.IPC;
using Rollback.World.Network.IPC.Handlers.Gifts;

namespace Rollback.World.Handlers.Connection
{
    public static class ConnectionHandler
    {
        [WorldHandler(AuthenticationTicketMessage.Id, false)]
        public static void HandleAuthenticationTicketMessage(WorldClient client, AuthenticationTicketMessage message)
        {
            IPCService.Instance.SendRequest<ClientConnectionAcceptedMessage, ClientConnectionRefusedMessage>(new ClientConnectionRequestMessage(message.ticket),
                ccam =>
                {
                    WorldServer.Instance.GetClient(x => x.Account?.Id == ccam.AccountId)?.Dispose();

                    Dictionary<int, CharacterRecord> characters = new();
                    foreach (var id in ccam.CharactersIds!)
                    {
                        var character = CharacterManager.GetCharacterRecordById(id);
                        if (character is not null && !characters.ContainsKey(id))
                            characters[id] = character;
                    }

                    Dictionary<int, Gift> gifts = new();
                    foreach (var gift in ccam.Gifts)
                    {
                        List<KeyValuePair<ItemRecord, int>> giftItems = new();
                        for (int i = 0; i < gift.Items.Length; i++)
                        {
                            var templateRecord = ItemManager.Instance.GetTemplateRecordById(gift.Items[i].Key);

                            if (templateRecord is not null)
                                giftItems.Add(new KeyValuePair<ItemRecord, int>(templateRecord, gift.Items[i].Value));
                            else
                            {
                                client.Dispose();
                                Logger.Instance.LogWarn($"Error while loading gifts for account {client.Account!.Id}, item {gift.Items[i].Key} doesn't exist");
                                return;
                            }
                        }

                        gifts.Add(gift.Id, new(gift.Id, gift.Title, gift.Description, giftItems));
                    }


                    client.Account = new(DatabaseAccessor.Instance.SelectSingle<AccountInformationsRecord>(string.Format(AccountInformationsRelator.GetAccountInformationsById, ccam.AccountId)),
                        ccam.AccountId, ccam.Nickname, (GameHierarchyEnum)ccam.Role, message.ticket, characters, ccam.SecretAnswer,
                        ccam.FreeCharacterSlots, ccam.LastConnection is < 0 ? default : ccam.LastConnection.GetDateTimeFromUnixTimeStamp(), ccam.LastIP, gifts);

                    SendAuthenticationTicketAcceptedMessage(client);
                    BasicHandler.SendBasicTimeMessage(client);
                },
                error => { client.Dispose(); });
        }

        [WorldHandler(CharactersListRequestMessage.Id, false)]
        public static void HandleCharactersListRequestMessage(WorldClient client, CharactersListRequestMessage message)
        {
            if (message is null)
                throw new ArgumentNullException(nameof(message));

            SendCharactersListMessage(client);
        }

        [WorldHandler(ClientKeyMessage.Id)]
        public static void HandleClientKeyMessage(WorldClient client, ClientKeyMessage message)
        {
            if (client is null || message is null)
                throw new ArgumentNullException(nameof(message));
        }

        [WorldHandler(StartupActionsExecuteMessage.Id, false)]
        public static void HandleStartupActionsExecuteMessage(WorldClient client, StartupActionsExecuteMessage message) =>
            SendStartupActionsListMessage(client);

        [WorldHandler(StartupActionsObjetAttributionMessage.Id, false)]
        public static void HandleStartupActionsObjetAttributionMessage(WorldClient client, StartupActionsObjetAttributionMessage message)
        {
            Gift? gift = default;

            if (client.Account!.Characters.ContainsKey(message.characterId) && client.Account.Gifts.TryGetValue(message.actionId, out gift))
            {
                IPCGiftHandler.SendGiftReceivedMessage(message.actionId);

                for (var i = 0; i < gift.Items.Count; i++)
                    ItemManager.Instance.CreatePlayerItem(null!, gift.Items[i].Key, gift.Items[i].Value, CustomEnums.EffectGenerationType.Normal).Save(message.characterId);
            }

            SendStartupActionFinishedMessage(client, gift is not null, message.actionId);
        }

        public static void SendHelloGameMessage(WorldClient client) =>
            client.Send(new HelloGameMessage());

        public static void SendAuthenticationTicketAcceptedMessage(WorldClient client) =>
            client.Send(new AuthenticationTicketAcceptedMessage());

        public static void SendCharactersListMessage(WorldClient client) =>
            client.Send(new CharactersListMessage(client.Account!.Gifts.Count is not 0, client.Account.Characters.Count is 0, client.Account.Characters.Values.OrderByDescending(x => x.LastSelection).Select(x => x.CharacterBaseInformations).ToArray()));

        public static void SendStartupActionsListMessage(WorldClient client) =>
            client.Send(new StartupActionsListMessage(client.Account!.Gifts.Values.Select(x => x.StartupActionAddObject).ToArray()));

        public static void SendStartupActionFinishedMessage(WorldClient client, bool success, int actionId) =>
            client.Send(new StartupActionFinishedMessage(success, false, actionId));
    }
}
