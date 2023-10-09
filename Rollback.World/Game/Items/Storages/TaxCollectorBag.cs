using Rollback.Common.ORM;
using Rollback.World.Database.TaxCollectors;
using Rollback.World.Game.Items.Types;
using Rollback.World.Game.RolePlayActors.TaxCollectors;
using Rollback.World.Handlers.Exchanges;

namespace Rollback.World.Game.Items.Storages
{
    public sealed class TaxCollectorBag : SaveableItemsStorage<TaxCollector, TaxCollectorItemRecord, TaxCollectorItem, TaxCollectorBag>
    {
        protected override int OwnerId =>
            Owner.Id;

        public override int MaxPods =>
            Owner.Guild.TaxCollectorPods;

        public TaxCollectorBag(TaxCollector owner) : base(owner)
        {

            ItemAdded += OnTaxCollectorItemAdded;
            ItemQuantityChanged += OnTaxCollectorItemQuantityChanged;
            ItemRemoved += OnTaxCollectorItemRemoved;
        }

        private void OnTaxCollectorItemAdded(TaxCollectorItem item)
        {
            if (Owner.Dialoger is not null)
                ExchangeHandler.SendStorageObjectUpdateMessage(Owner.Dialoger.Client, item.ObjectItem);
        }

        private void OnTaxCollectorItemQuantityChanged(TaxCollectorItem item)
        {
            if (Owner.Dialoger is not null)
                ExchangeHandler.SendStorageObjectUpdateMessage(Owner.Dialoger.Client, item.ObjectItem);
        }

        private void OnTaxCollectorItemRemoved(TaxCollectorItem item, bool delete)
        {
            if (Owner.Dialoger is not null)
                ExchangeHandler.SendStorageObjectRemoveMessage(Owner.Dialoger.Client, item.UID);
        }

        protected override void Load()
        {
            foreach (var item in DatabaseAccessor.Instance.Select<TaxCollectorItemRecord>(string.Format(TaxCollectorItemRelator.GetItemsByTaxCollectorId, OwnerId)))
            {
                if (item.Template is null)
                    throw new Exception($"Can not find template {item.ItemId} for item {item.UID}, in tax collector items.");

                _items.TryAdd(item.UID, new TaxCollectorItem(item, this));
            }
        }
    }
}
