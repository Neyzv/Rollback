using Rollback.Protocol.Enums;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Items;
using Rollback.World.Game.Interactions.Dialogs.Exchanges.Trades.Offers;
using Rollback.World.Game.Interactions.Dialogs.Exchanges.Trades.Traders;
using Rollback.World.Game.Items;
using Rollback.World.Game.Items.Storages;
using Rollback.World.Game.Items.Types;
using Rollback.World.Handlers.Exchanges;

namespace Rollback.World.Game.Interactions.Dialogs.Exchanges.Trades.Types
{
    public class NpcTrade : Trade<NpcTrader>
    {
        protected readonly List<TradeOffer> _tradeOffers;

        public override ExchangeTypeEnum ExchangeType =>
            ExchangeTypeEnum.NPC_TRADE;

        public NpcTrade(PlayerTrader sender, NpcTrader receiver, List<TradeOffer> tradeOffers)
            : base(sender, receiver)
        {
            _tradeOffers = tradeOffers;
            ItemMoved += OnItemMoved;
            KamasChanged += OnKamasChanged;
        }

        protected override void InternalOpen() =>
            ExchangeHandler.SendExchangeStartOkNpcTradeMessage(Sender.Character.Client, Receiver.Id);

        protected virtual PlayerItem? CreateTraderItem(ItemRecord template, int quantity, TradeOffer tradeOffer) =>
            ItemManager.Instance.CreatePlayerItem(Sender.Character.Inventory,
                template,
                quantity,
                EffectGenerationType.Normal
            );

        private void CheckTradedItems()
        {
            ClearItems(Receiver);

            if (Sender.Items.Count is not 0 && _tradeOffers.FirstOrDefault(x => Sender.Items.Where(y => x.NeededItemsInformations.ContainsKey(y.Id) &&
                    x.NeededItemsInformations[y.Id] == y.Quantity).Count() == x.NeededItemsInformations.Count && x.NeededKamas == Receiver.Kamas) is { } tradeOffer)
            {
                if (tradeOffer.GivedKamas > 0)
                    Receiver.ChangeKamas(tradeOffer.GivedKamas);

                foreach (var (itemToGive, quantity) in tradeOffer.GivedItemsInformations)
                    if (ItemManager.Instance.GetTemplateRecordById(itemToGive) is { } template)
                    {
                        if (CreateTraderItem(template, quantity, tradeOffer) is { } playerItem)
                        {
                            Receiver.AddItem(playerItem);
                            MoveItem(Receiver.Id, playerItem.UID, playerItem.Quantity);
                        }
                        else
                        {
                            ClearItems(Receiver);
                            Sender.Character.SendServerMessage($"Error while creating item {itemToGive} to receive...");
                            break;
                        }
                    }
                    else
                    {
                        ClearItems(Receiver);
                        Sender.Character.SendServerMessage($"Item {itemToGive} to give doesn't exist...");
                        break;
                    }
            }
        }

        private void OnItemMoved(Trader trader, TradeItem item, bool _)
        {
            if (trader.Id == Sender.Id)
                CheckTradedItems();
        }

        private void OnKamasChanged(Trader trader, int kamas)
        {
            if (trader.Id == Sender.Id)
                CheckTradedItems();
        }

        protected override void InternalProcessTrade()
        {
            if (Sender.Character.Kamas + Receiver.Kamas - Sender.Kamas <= Inventory.MaxKamasInInventory)
            {
                if (Sender.Character.Inventory.Pods + Receiver.Items.Sum(x => x.StackPod) <= Sender.Character.Inventory.MaxPods)
                {
                    foreach (var item in Sender.Items)
                        Sender.Character.Inventory.RemoveItem(item.UID, item.Quantity);

                    foreach (var item in Receiver.Items)
                        item.AddToInventory(Sender.Character.Inventory);
                }
                else
                    // Ton inventaire est plein, impossible d\'y ajouter d\'autres objets.
                    Sender.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 10);
            }
        }
    }
}
