using Rollback.Common.ORM;

namespace Rollback.World.Database.Items;

public static class ItemBreedingRelator
{
    public const string GetAllDatas = "SELECT * FROM items_breeding";
}

[Table("items_breeding")]
public sealed record ItemBreedingRecord
{
    [Key]
    public short ItemId { get; set; }
    
    public short MaxDurability { get; set; }
}