using Rollback.Common.ORM;

namespace Rollback.World.Database.World
{
    public static class AreaRelator
    {
        public const string GetAreas = "SELECT * FROM world_areas";
    }

    [Table("world_areas")]
    public sealed record AreaRecord
    {
        [Key]
        public short Id { get; set; }

        public short SuperAreaId { get; set; }
    }
}
