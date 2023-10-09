using System.Diagnostics.CodeAnalysis;
using Rollback.World.Game.Items.Types;

namespace Rollback.World.Game.Interactions.Dialogs.Exchanges.Trades
{
    public abstract class Trader
    {
        protected List<TradeItem> _items;

        public abstract int Id { get; }

        public int Kamas { get; protected set; }

        public IReadOnlyCollection<TradeItem> Items =>
            _items;

        public virtual bool Ready { get; set; }

        public Trader() =>
            _items = new List<TradeItem>();

        protected event Action? ClearItems;
        public void OnClearItems() =>
            ClearItems?.Invoke();

        protected abstract PlayerItem? RetrieveFromStorageByUID(int uid);

        public virtual void UpdateReadyState(int actorId, bool state) { }

        public virtual void UpdateKamas(int actorId, int amount) { }

        public abstract bool ChangeKamas(int amount);

        public virtual void UpdateItem(int actorId, TradeItem item, bool modified) { }

        public virtual bool MoveItem(int uid, int quantity, [NotNullWhen(true)] out TradeItem? tradeItem, out bool modified)
        {
            bool result = false;
            tradeItem = default;
            modified = false;

            var item = RetrieveFromStorageByUID(uid);
            if (item is not null && !item.IsEquipped)
            {
                tradeItem = Items.FirstOrDefault(x => x.UID == uid);

                if (tradeItem is not null)
                {
                    if (tradeItem.Quantity + quantity <= item.Quantity)
                    {
                        if ((tradeItem.Quantity += quantity) <= 0)
                            _items.Remove(tradeItem);
                        else
                            modified = true;

                        result = true;
                    }
                    else
                        tradeItem = default;
                }
                else if (quantity > 0 && quantity <= item.Quantity)
                {
                    tradeItem = new TradeItem(item, quantity);
                    _items.Add(tradeItem);

                    result = true;
                }
            }

            return result;
        }
    }
}
