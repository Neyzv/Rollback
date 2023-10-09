using Rollback.Common.ORM;

namespace Rollback.World.Database.Monsters
{
    public static class MonsterSpellRelator
    {
        public const string GetSpellByMonsterAndGradeId = "SELECT * FROM monsters_spells WHERE MonsterId = {0} AND GradeId = {1}";
    }

    [Table("monsters_spells")]
    public sealed record MonsterSpellRecord
    {
        [Key]
        public short MonsterId { get; set; }

        [Key]
        public sbyte GradeId { get; set; }

        [Key]
        public short SpellId { get; set; }

        public sbyte SpellLevel { get; set; }
    }
}
