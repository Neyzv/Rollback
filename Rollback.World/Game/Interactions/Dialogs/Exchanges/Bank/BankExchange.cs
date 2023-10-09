using Rollback.Protocol.Enums;
using Rollback.World.Game.Items.Storages;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Handlers.Exchanges;
using Rollback.World.Handlers.Inventory;

namespace Rollback.World.Game.Interactions.Dialogs.Exchanges.Bank
{
    public sealed class BankExchange : Exchange<Character>
    {
        public override ExchangeTypeEnum ExchangeType =>
            ExchangeTypeEnum.STORAGE;

        public BankExchange(Character character, Character dialoger) : base(character, dialoger) { }

        protected override void InternalOpen()
        {
            ExchangeHandler.SendExchangeStartedMessage(Character.Client, ExchangeType);
            InventoryHandler.SendStorageInventoryContentMessage(Character.Client, Character.Bank.InventoryContent, Character.Bank.Kamas);
        }

        protected override void InternalClose() =>
            ExchangeHandler.SendExchangeLeaveMessage(Character.Client, false);

        public override void SetKamas(int actorId, int amount)
        {
            if (amount is not 0 && (Character.Bank.Kamas + amount <= Inventory.MaxKamasInInventory || Character.Kamas + amount <= Inventory.MaxKamasInInventory) &&
                (Character.Bank.Kamas + amount >= 0 || Character.Kamas + amount >= 0))
            {
                Character.Bank.ChangeKamas(amount);
                Character.ChangeKamas(-amount, false);
            }
        }

        public override void MoveItem(int actorId, int uid, int quantity)
        {
            if (quantity > 0)
                Character.Bank.StoreItem(uid, quantity);
            else if (quantity < 0)
                Character.Bank.TakeItemBack(uid, Math.Abs(quantity));
        }
    }
}
