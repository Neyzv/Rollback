using Rollback.Common.ORM;

namespace Rollback.World.Database.Monsters
{
    public static class MonsterSpawnRelator
    {
        public const string GetSpawns = "SELECT * FROM monsters_spawns";
    }

    [Table("monsters_spawns")]
    public sealed record MonsterSpawnRecord
    {
        [Key]
        public int Id { get; set; }

        public short? SubAreaId { get; set; }

        public int? MapId { get; set; }

        public short MonsterId { get; set; }

        public sbyte MinGrade { get; set; }

        public sbyte MaxGrade { get; set; }

        public byte Probability { get; set; }

        public bool Disabled { get; set; }
    }
}
