using Rollback.Common.ORM;
using Rollback.World.Game.Items;

namespace Rollback.World.Database.Characters
{
    public static class CharacterItemRelator
    {
        public const string GetByOwnerId = "SELECT * FROM characters_items WHERE OwnerId = {0}";
        public const string GetMaxUID = "SELECT MAX(UID) MaxUID FROM characters_items";
        public const string DeleteByOwnerId = "DELETE FROM characters_items WHERE OwnerId = {0}";
    }

    [Table("characters_items")]
    public sealed record CharacterItemRecord : ItemBaseRecord
    {
        public byte Position { get; set; }
    }
}
