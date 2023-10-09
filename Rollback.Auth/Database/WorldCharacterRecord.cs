using Rollback.Common.ORM;

namespace Rollback.Auth.Database
{
    public static class WorldCharacterRelator
    {
        public const string GetWorldsCharactersByAccountId = "SELECT * FROM worlds_characters WHERE AccountId = {0}";
        public const string DeleteWorldCharacterByWorldAndCharacterId = "DELETE FROM worlds_characters WHERE WorldId = {0} AND CharacterId = {1}";
    }

    [Table("worlds_characters")]
    public sealed record WorldCharacterRecord
    {
        public int CharacterId { get; set; }

        public int AccountId { get; set; }

        public int WorldId { get; set; }
    }
}
