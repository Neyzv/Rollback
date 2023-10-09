using Rollback.Protocol.Messages;
using Rollback.Protocol.Types;
using Rollback.World.Game.Interactions;
using Rollback.World.Game.Interactions.Dialogs.Npcs;
using Rollback.World.Game.RolePlayActors;
using Rollback.World.Game.RolePlayActors.Npcs;
using Rollback.World.Game.RolePlayActors.TaxCollectors;
using Rollback.World.Network;
using Rollback.World.Network.Handler;

namespace Rollback.World.Handlers.Npcs
{
    public static class NpcHandler
    {
        [WorldHandler(NpcGenericActionRequestMessage.Id)]
        public static void HandleNpcGenericActionRequestMessage(WorldClient client, NpcGenericActionRequestMessage message)
        {
            if (!client.Account!.Character!.IsBusy)
            {
                var actor = client.Account.Character.MapInstance?.GetActor<RolePlayActor>(message.npcId);
                if (actor is not null)
                {
                    if (actor is Npc npc)
                        npc.ExecuteBestAction(client.Account.Character, message.npcActionId);
                    else if (actor is TaxCollectorNpc taxCollector)
                        new TaxCollectorDialog(client.Account.Character, taxCollector).Open();
                }
            }
        }

        [WorldHandler(LeaveDialogRequestMessage.Id)]
        public static void HandleLeaveDialogRequestMessage(WorldClient client, LeaveDialogRequestMessage message) =>
            client.Account!.Character!.Interaction?.Close();

        [WorldHandler(NpcDialogReplyMessage.Id)]
        public static void HandleNpcDialogReplyMessage(WorldClient client, NpcDialogReplyMessage message)
        {
            if (client.Account!.Character!.Interaction is NpcDialog dialog)
                dialog.ExecuteReply(message.replyId);
        }

        [WorldHandler(ExchangeBuyMessage.Id)]
        public static void HandleExchangeBuyMessage(WorldClient client, ExchangeBuyMessage message)
        {
            if (client.Account!.Character!.Interaction is IShop shop)
                shop.BuyItem(message.objectToBuyId, message.quantity);
        }

        [WorldHandler(ExchangeSellMessage.Id)]
        public static void HandleExchangeSellMessage(WorldClient client, ExchangeSellMessage message)
        {
            if (client.Account!.Character!.Interaction is IShop shop)
                shop.SellItem(message.objectToSellId, message.quantity);
        }

        public static void SendNpcDialogCreationMessage(WorldClient client, ContextualActor actor) =>
            client.Send(new NpcDialogCreationMessage(actor.MapInstance.Map.Record.Id, actor.Id));

        public static void SendLeaveDialogMessage(WorldClient client) =>
            client.Send(new LeaveDialogMessage());

        public static void SendNpcDialogQuestionMessage(WorldClient client, params string[] parameters)
        {
            var dialog = client.Account!.Character!.Interaction as NpcDialog;
            if (dialog is not null && dialog.CurrentMessageId.HasValue)
            {
                var replies = new HashSet<short>();

                foreach (var reply in dialog.Replies)
                    if (!replies.Contains(reply.ReplyId))
                        replies.Add(reply.ReplyId);

                client.Send(new NpcDialogQuestionMessage(dialog.CurrentMessageId.Value, parameters, replies.ToArray()));
            }
        }

        public static void SendExchangeStartOkNpcShopMessage(WorldClient client, int npcUID, ObjectItemToSellInNpcShop[] items) =>
            client.Send(new ExchangeStartOkNpcShopMessage(npcUID, items));

        public static void SendSpellForgetUIMessage(WorldClient client, bool open) =>
            client.Send(new SpellForgetUIMessage(open));

        public static void SendTaxCollectorDialogQuestionExtendedMessage(WorldClient client, TaxCollectorNpc taxCollector) =>
            client.Send(new TaxCollectorDialogQuestionExtendedMessage(taxCollector.TaxCollector.Guild.Name,
                taxCollector.TaxCollector.Guild.TaxCollectorPods, taxCollector.TaxCollector.Guild.TaxCollectorProspecting,
                taxCollector.TaxCollector.Guild.TaxCollectorWisdom, (sbyte)taxCollector.TaxCollector.Guild.GetTaxCollectors().Length));

        public static void SendEntityTalkMessage(WorldClient client, Npc npc, short textId, params string[] parameters) =>
            client.Send(new EntityTalkMessage(npc.Id, textId, parameters));
    }
}
