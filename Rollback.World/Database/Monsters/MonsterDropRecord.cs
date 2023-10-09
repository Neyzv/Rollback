using Rollback.Common.ORM;

namespace Rollback.World.Database.Monsters
{
    public static class MonsterDropRelator
    {
        public const string GetDropsByMonsterId = "SELECT * FROM monsters_drops WHERE MonsterId = {0}";
    }

    [Table("monsters_drops")]
    public sealed record MonsterDropRecord
    {
        [Key]
        public short MonsterId { get; set; }

        [Key]
        public short ItemId { get; set; }

        public float Percentage { get; set; }

        public short LootingFloor { get; set; }

        public short MaxAmountPerMonster { get; set; }

        public short MaxAmountPerFight { get; set; }
    }
}
