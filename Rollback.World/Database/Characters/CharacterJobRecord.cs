using Rollback.Common.ORM;
using Rollback.World.CustomEnums;

namespace Rollback.World.Database.Characters
{
    public static class CharacterJobRelator
    {
        public const string GetJobsByOwnerId = "SELECT * FROM characters_jobs WHERE OwnerId = {0}";
    }

    [Table("characters_jobs")]
    public sealed record CharacterJobRecord
    {
        [Key]
        public int OwnerId { get; set; }

        [Key]
        public JobIds JobId { get; set; }

        public long Experience { get; set; }
    }
}
