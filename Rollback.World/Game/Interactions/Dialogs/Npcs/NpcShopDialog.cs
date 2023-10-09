using Rollback.Protocol.Enums;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Npcs;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.RolePlayActors.Npcs;
using Rollback.World.Handlers.Exchanges;
using Rollback.World.Handlers.Npcs;

namespace Rollback.World.Game.Interactions.Dialogs.Npcs
{
    public sealed class NpcShopDialog : Dialog<Npc>, IShop
    {
        private readonly Dictionary<short, NpcItemRecord> _items;

        private readonly bool _canSell;
        public bool CanSell =>
            _canSell;

        public NpcShopDialog(Character character, Npc dialoger, Dictionary<short, NpcItemRecord> items, bool canSell) : base(character, dialoger)
        {
            _items = items;
            _canSell = canSell;
        }

        public override DialogType DialogType =>
            DialogType.Exchange;

        protected override void InternalOpen() =>
            NpcHandler.SendExchangeStartOkNpcShopMessage(Character.Client, Dialoger.Id, _items.Values.Select(x => x.ObjectItemToSellInNpcShop).ToArray());

        protected override void InternalClose() =>
            ExchangeHandler.SendExchangeLeaveMessage(Character.Client, false);

        public void BuyItem(int id, int quantity)
        {
            if (quantity > 0 && _items.TryGetValue((short)id, out var item))
            {
                if (!Character.Inventory.IsFull)
                {
                    var finalPrice = item.Price * quantity;
                    if (Character.Kamas >= finalPrice)
                    {
                        Character.ChangeKamas(-finalPrice, false);
                        Character.Inventory.AddItem(item.ItemId, quantity, item.EffectGenerationType);

                        ExchangeHandler.SendExchangeBuyOkMessage(Character.Client);
                    }
                    else
                        //Vous ne disposez pas d\'assez de kamas pour acheter cet objet.
                        Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 63);
                }
                else
                    //Ton inventaire est plein, impossible d\'y ajouter d\'autres objets.
                    Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 10);
            }
            else
                ExchangeHandler.SendExchangeErrorMessage(Character.Client, ExchangeErrorEnum.BUY_ERROR);
        }

        public void SellItem(int id, int quantity)
        {
            if (CanSell)
            {
                var itemToSell = Character.Inventory.GetItemByUID(id);
                if (itemToSell is not null && quantity > 0)
                {
                    if (itemToSell.Quantity >= quantity)
                    {
                        Character.Inventory.RemoveItem(itemToSell, quantity);
                        Character.ChangeKamas((int)(Math.Ceiling(itemToSell.Price / 10d) * quantity), false);

                        //Tu as perdu %1 \'$item%2\'.
                        Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 22, quantity, itemToSell.Id);
                    }
                    else
                        //Tu ne possèdes pas l\'objet nécessaire.
                        Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 4);
                }
            }
        }
    }
}
