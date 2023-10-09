using Rollback.Common.ORM;

namespace Rollback.World.Database.World
{
    public static class SubAreaRelator
    {
        public const string GetSubAreas = "SELECT * FROM world_subareas";
    }

    [Table("world_subareas")]
    public sealed record SubAreaRecord
    {
        [Key]
        public short Id { get; set; }

        public short AreaId { get; set; }
    }
}
