using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Effects;
using Rollback.World.Game.Items.Storages;
using Rollback.World.Game.Items.Types;

namespace Rollback.World.Game.Interactions.Dialogs.Exchanges.Trades
{
    public class TradeItem
    {
        protected readonly PlayerItem _item;

        public int UID =>
            _item.UID;

        public short Id =>
            _item.Id;

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;

                if (_quantity > _item.Quantity)
                    _quantity = _item.Quantity;
            }
        }

        public ItemType TypeId =>
            _item.TypeId;

        public virtual ObjectItem ObjectItem
        {
            get
            {
                var objectItem = _item.ObjectItem;
                objectItem.quantity = Quantity;

                return objectItem;
            }
        }

        public int StackPod =>
            _item.Weight * Quantity;

        public IReadOnlyCollection<EffectBase> Effects =>
            _item.Effects;

        public TradeItem(PlayerItem item, int quantity)
        {
            _item = item;
            Quantity = quantity;
        }

        public void AddToInventory(Inventory inventory) =>
            inventory.AddItem(_item);
    }
}
