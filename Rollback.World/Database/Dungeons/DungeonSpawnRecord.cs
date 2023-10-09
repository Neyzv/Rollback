using Rollback.Common.ORM;

namespace Rollback.World.Database.Dungeons
{
    public sealed class DungeonSpawnRelator
    {
        public const string GetDungeonSpawnsByDungeonId = "SELECT * FROM dungeons_spawns WHERE DungeonId = {0}";
    }

    [Table("dungeons_spawns")]
    public sealed record DungeonSpawnRecord
    {
        [Key]
        public int Id { get; set; }

        public int DungeonId { get; set; }

        public short MonsterId { get; set; }

        public sbyte? Grade { get; set; }

        public byte Members { get; set; }
    }
}
