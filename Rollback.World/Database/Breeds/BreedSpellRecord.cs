using Rollback.Common.ORM;

namespace Rollback.World.Database.Breeds
{
    public static class BreedSpellRelator
    {
        public const string GetBreedsSpells = "SELECT * FROM breeds_spells";
    }

    [Table("breeds_spells")]
    public sealed record BreedSpellRecord
    {
        public int BreedId { get; set; }

        public short SpellId { get; set; }
    }
}
