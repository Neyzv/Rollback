using Rollback.Common.ORM;

namespace Rollback.World.Database.World
{
    public static class SuperAreaRelator
    {
        public const string GetSuperAreas = "SELECT * FROM world_superareas";
    }

    [Table("world_superareas")]
    public sealed record SuperAreaRecord
    {
        [Key]
        public short Id { get; set; }

        public byte WorldMapId { get; set; }
    }
}
