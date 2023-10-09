using System.Diagnostics.CodeAnalysis;
using Rollback.Common.ORM;

namespace Rollback.World.Database.Monsters
{
    public static class MonsterRelator
    {
        public const string GetMonsters = "SELECT * FROM monsters_templates";
    }

    [Table("monsters_templates")]
    public sealed record MonsterRecord
    {
        public MonsterRecord() =>
            (EntityLookString, Grades) = (string.Empty, new());

        [Key]
        public short Id { get; set; }

        public string EntityLookString { get; set; }

        public byte Race { get; set; }

        [Ignore]
        public List<MonsterGradeRecord> Grades { get; set; }

        [Ignore, NotNull]
        public List<MonsterDropRecord>? Drops { get; set; }
    }
}
