using Rollback.Common.ORM;
using Rollback.World.Game.Items;

namespace Rollback.World.Database.TaxCollectors
{
    public static class TaxCollectorItemRelator
    {
        public const string GetItemsByTaxCollectorId = "SELECT * FROM tax_collectors_items WHERE OwnerId = {0}";
        public const string DeleteByOwnerId = "DELETE FROM tax_collectors_items WHERE OwnerId = {0}";
    }

    [Table("tax_collectors_items")]
    public sealed record TaxCollectorItemRecord : ItemBaseRecord;
}
