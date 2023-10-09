using Rollback.Common.ORM;

namespace Rollback.World.Database.Spells
{
    public static class SpellTemplateRelator
    {
        public const string GetSpellTemplates = "SELECT * FROM spells_templates";
    }

    [Table("spells_templates")]
    public record SpellTemplateRecord
    {
        public SpellTemplateRecord() =>
            (SpellLevelsCSV, SpellLevels) = (string.Empty, Array.Empty<SpellLevelRecord>());

        [Key]
        public short Id { get; set; }

        public sbyte TypeId { get; set; }

        public string SpellLevelsCSV { get; set; }

        [Ignore]
        public SpellLevelRecord[] SpellLevels { get; set; }
    }
}
