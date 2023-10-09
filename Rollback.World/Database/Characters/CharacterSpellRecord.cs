using Rollback.Common.ORM;

namespace Rollback.World.Database.Characters
{
    public static class CharacterSpellRelator
    {
        public const string GetByOwnerId = "SELECT * FROM characters_spells WHERE OwnerId = {0}";
        public const string DeleteByOwnerId = "DELETE FROM characters_spells WHERE OwnerId = {0}";
    }

    [Table("characters_spells")]
    public record CharacterSpellRecord
    {
        [Key]
        public int OwnerId { get; set; }

        [Key]
        public short SpellId { get; set; }

        public sbyte SpellLevel { get; set; }

        public byte Position { get; set; }
    }
}
