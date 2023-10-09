using Rollback.World.Database.TaxCollectors;
using Rollback.World.Game.Items.Storages;
using Rollback.World.Game.RolePlayActors.TaxCollectors;

namespace Rollback.World.Game.Items.Types
{
    public sealed class TaxCollectorItem : SaveableItem<TaxCollectorItem, TaxCollectorItemRecord, TaxCollector, TaxCollectorBag>
    {
        public TaxCollectorItem(TaxCollectorItemRecord record, TaxCollectorBag taxBag)
            : base(record, taxBag) { }
    }
}
