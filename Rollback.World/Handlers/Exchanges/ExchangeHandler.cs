using Rollback.Protocol.Enums;
using Rollback.Protocol.Messages;
using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Interactions.Dialogs.Exchanges;
using Rollback.World.Game.Interactions.Dialogs.Exchanges.Trades;
using Rollback.World.Game.Interactions.Dialogs.Exchanges.Trades.Types;
using Rollback.World.Game.Interactions.Requests;
using Rollback.World.Game.Interactions.Requests.Exchanges;
using Rollback.World.Game.Interactives;
using Rollback.World.Game.Items.Types;
using Rollback.World.Game.RolePlayActors;
using Rollback.World.Network;
using Rollback.World.Network.Handler;

namespace Rollback.World.Handlers.Exchanges
{
    public static class ExchangeHandler
    {
        [WorldHandler(ExchangePlayerRequestMessage.Id)]
        public static void HandleExchangePlayerRequestMessage(WorldClient client, ExchangePlayerRequestMessage message)
        {
            if (client.Account!.Character!.Fighter is null)
            {
                var target = WorldServer.Instance.GetClient(x => x.Account?.Character?.Id == message.target)?.Account!.Character;
                if (target is not null)
                {
                    if (target.MapInstance == client.Account!.Character!.MapInstance)
                    {
                        if (!target.IsBusy && !client.Account.Character.IsBusy) // TO DO Ignored
                            new ExchangeRequest(client.Account.Character, target).Open();
                        else
                            SendExchangeErrorMessage(client, ExchangeErrorEnum.REQUEST_CHARACTER_OCCUPIED);
                    }
                    else
                        SendExchangeErrorMessage(client, ExchangeErrorEnum.REQUEST_CHARACTER_TOOL_TOO_FAR);
                }
                else
                    SendExchangeErrorMessage(client, ExchangeErrorEnum.BID_SEARCH_ERROR);
            }
        }

        [WorldHandler(ExchangeAcceptMessage.Id)]
        public static void HandleExchangeAcceptMessage(WorldClient client, ExchangeAcceptMessage message)
        {
            if (client.Account!.Character!.Interaction is Request request)
                request.Accept();
        }

        [WorldHandler(ExchangeObjectMoveKamaMessage.Id)]
        public static void HandleExchangeObjectMoveKamaMessage(WorldClient client, ExchangeObjectMoveKamaMessage message)
        {
            if (client.Account!.Character!.Interaction is IExchange trade)
                trade.SetKamas(client.Account.Character.Id, message.quantity);
        }

        [WorldHandler(ExchangeObjectMoveMessage.Id)]
        public static void HandleExchangeObjectMoveMessage(WorldClient client, ExchangeObjectMoveMessage message)
        {
            if (client.Account!.Character!.Interaction is IExchange trade)
                trade.MoveItem(client.Account.Character.Id, message.objectUID, message.quantity);
        }

        [WorldHandler(ExchangeReadyMessage.Id)]
        public static void HandleExchangeReadyMessage(WorldClient client, ExchangeReadyMessage message)
        {
            if (client.Account!.Character!.Interaction is ITrade trade)
                trade.ChangeReadyState(client.Account.Character.Id, message.ready);
        }

        [WorldHandler(ExchangeReplayMessage.Id)]
        public static void HandleExchangeReplayMessage(WorldClient client, ExchangeReplayMessage message)
        {
            if (client.Account!.Character!.Interaction is CraftTrade trade)
                trade.ReplayTrade(message.count);
        }

        public static void SendExchangeStartedMessage(WorldClient client, ExchangeTypeEnum exchangeType) =>
            client.Send(new ExchangeStartedMessage((sbyte)exchangeType));

        public static void SendExchangeLeaveMessage(WorldClient client, bool success) =>
            client.Send(new ExchangeLeaveMessage(success));

        public static void SendExchangeBuyOkMessage(WorldClient client) =>
            client.Send(new ExchangeBuyOkMessage());

        public static void SendExchangeErrorMessage(WorldClient client, ExchangeErrorEnum reason) =>
            client.Send(new ExchangeErrorMessage((sbyte)reason));

        public static void SendExchangeRequestedTradeMessage(WorldClient client, ExchangeTypeEnum type, int senderId, int receiverId) =>
            client.Send(new ExchangeRequestedTradeMessage((sbyte)type, senderId, receiverId));

        public static void SendExchangeKamaModifiedMessage(WorldClient client, bool isOther, int kamas) =>
            client.Send(new ExchangeKamaModifiedMessage(isOther, kamas));

        public static void SendExchangeObjectRemovedMessage(WorldClient client, bool isOther, int objectUID) =>
            client.Send(new ExchangeObjectRemovedMessage(isOther, objectUID));

        public static void SendExchangeObjectModifiedMessage(WorldClient client, bool isOther, ObjectItem item) =>
            client.Send(new ExchangeObjectModifiedMessage(isOther, item));

        public static void SendExchangeObjectAddedMessage(WorldClient client, bool isOther, ObjectItem item) =>
            client.Send(new ExchangeObjectAddedMessage(isOther, item));

        public static void SendExchangeIsReadyMessage(WorldClient client, int traderId, bool state) =>
            client.Send(new ExchangeIsReadyMessage(traderId, state));

        public static void SendStorageKamasUpdateMessage(WorldClient client, int amount) =>
            client.Send(new StorageKamasUpdateMessage(amount));

        public static void SendStorageObjectUpdateMessage(WorldClient client, ObjectItem item) =>
            client.Send(new StorageObjectUpdateMessage(item));

        public static void SendStorageObjectRemoveMessage(WorldClient client, int uid) =>
            client.Send(new StorageObjectRemoveMessage(uid));

        public static void SendExchangeStartOkCraftWithInformationMessage(WorldClient client, Skill skill) =>
            client.Send(new ExchangeStartOkCraftWithInformationMessage(2, (int)skill.TemplateId!));

        public static void SendExchangeCraftResultMessage(WorldClient client, ExchangeCraftResult result) =>
            client.Send(new ExchangeCraftResultMessage((sbyte)result));

        public static void SendExchangeCraftResultWithObjectDescMessage(WorldClient client, ExchangeCraftResult result, PlayerItem item) =>
            client.Send(new ExchangeCraftResultWithObjectDescMessage((sbyte)result, item.ObjectItemMinimalInformation));

        public static void SendExchangeCraftResultWithObjectIdMessage(WorldClient client, ExchangeCraftResult result, short itemId) =>
            client.Send(new ExchangeCraftResultWithObjectIdMessage((sbyte)result, itemId));

        public static void SendExchangeCraftInformationObjectMessage(WorldClient client, ExchangeCraftResult result, short itemId,
            RolePlayActor actor) =>
            client.Send(new ExchangeCraftInformationObjectMessage((sbyte)result, itemId, actor.Id));

        public static void SendExchangeStartOkNpcTradeMessage(WorldClient client, int npcId) =>
            client.Send(new ExchangeStartOkNpcTradeMessage(npcId));
    }
}
