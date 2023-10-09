using Rollback.Common.ORM;

namespace Rollback.World.Database.Dungeons
{
    public sealed class DungeonRelator
    {
        public const string GetDungeons = "SELECT * FROM dungeons";
    }

    [Table("dungeons")]
    public sealed record DungeonRecord
    {
        [Key]
        public int Id { get; set; }

        public int FightMapId { get; set; }

        public short? SpawnCellId { get; set; }

        public int TeleportMapId { get; set; }

        public short TeleportCellId { get; set; }
    }
}
