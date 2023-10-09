using Rollback.Common.ORM;
using Rollback.Protocol.Types;
using Rollback.World.Game.Items;

namespace Rollback.World.Database.World;

public static class PaddockItemRelator
{
    public const string GetItemsByMapId = "SElECT * FROM world_paddocks_items WHERE MapId = {0}";
}

[Table("world_paddocks_items")]
public sealed record PaddockItemRecord
{
    [Key]
    public int OwnerId { get; set; }
    
    [Key]
    public int MapId { get; set; }
    
    [Key]
    public short CellId { get; set; }
    
    public short ItemId { get; set; }
    
    public short Durability { get; set; }

    [Ignore]
    public PaddockItem PaddockItem =>
        new(CellId, ItemId, new ItemDurability(Durability, ItemManager.Instance.GetBreedingItemDurability(ItemId)!.Value));
}