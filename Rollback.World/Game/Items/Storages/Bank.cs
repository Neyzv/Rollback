using Rollback.Common.ORM;
using Rollback.Protocol.Enums;
using Rollback.World.Database.Accounts.Bank;
using Rollback.World.Game.Effects;
using Rollback.World.Game.Items.Types;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Handlers.Exchanges;
using Rollback.World.Handlers.Inventory;

namespace Rollback.World.Game.Items.Storages
{
    public sealed class Bank : SaveableItemsStorage<Character, AccountBankItemRecord, BankItem, Bank>
    {
        protected override int OwnerId =>
            Owner.Client.Account!.Id;

        public override int MaxPods =>
            0;

        public int Kamas { get; private set; }

        public Bank(Character owner) : base(owner)
        {
            Kamas = owner.Client.Account!.BankKamas;

            ItemAdded += OnBankItemAdded;
            ItemQuantityChanged += OnBankItemQuantityChanged;
            ItemRemoved += OnBankItemRemoved;
        }

        private void OnBankItemAdded(BankItem item) =>
            ExchangeHandler.SendStorageObjectUpdateMessage(Owner.Client, item.ObjectItem);

        private void OnBankItemQuantityChanged(BankItem item) =>
            ExchangeHandler.SendStorageObjectUpdateMessage(Owner.Client, item.ObjectItem);

        private void OnBankItemRemoved(BankItem item, bool delete) =>
            ExchangeHandler.SendStorageObjectRemoveMessage(Owner.Client, item.UID);

        protected override void Load()
        {
            foreach (var item in DatabaseAccessor.Instance.Select<AccountBankItemRecord>(string.Format(AccountBankItemRelator.GetItemsByAccountId, OwnerId)))
            {
                if (item.Template is null)
                    throw new Exception($"Can not find template {item.ItemId} for item {item.UID}, in bank.");

                AddItem(new BankItem(item, this));
            }
        }

        public void ChangeKamas(int amount)
        {
            var kamas = amount;

            if (Kamas + amount > Inventory.MaxKamasInInventory)
                kamas = Inventory.MaxKamasInInventory - Kamas;
            else if (Kamas + amount < 0)
                kamas = Math.Abs(Kamas);

            Kamas += kamas;

            ExchangeHandler.SendStorageKamasUpdateMessage(Owner.Client, Kamas);
        }

        public void StoreItem(int uid, int quantity)
        {
            if (quantity is not 0)
            {
                var absQuantity = Math.Abs(quantity);
                var item = Owner.Inventory.GetItemByUID(uid);
                if (item is not null && !item.IsEquipped && absQuantity <= item.Quantity)
                {
                    Owner.Inventory.RemoveItem(item, quantity, false);

                    AddItem(new(new()
                    {
                        UID = item.UID,
                        ItemId = item.Id,
                        Effects = EffectManager.SerializeEffects(item.Effects),
                        Quantity = quantity,
                        LastInteractionDate = DateTime.Now
                    }, this));
                }
                else
                    //Tu ne possèdes pas l\'objet nécessaire.
                    Owner.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 4);
            }
        }

        public void TakeItemBack(int uid, int quantity) =>
            MoveItemToCharacterInventory(Owner, uid, quantity);

        public override void Refresh()
        {
            ExchangeHandler.SendStorageKamasUpdateMessage(Owner.Client, Kamas);
            InventoryHandler.SendStorageInventoryContentMessage(Owner.Client, InventoryContent, Kamas);
        }
    }
}
