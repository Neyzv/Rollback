using Rollback.Common.ORM;
using Rollback.Protocol.Enums;

namespace Rollback.Auth.Database
{
    public static class WorldRelator
    {
        public const string GetQueries = "SELECT * FROM worlds";
    }

    [Table("worlds")]
    public record WorldRecord
    {
        [Key]
        public int Id { get; set; }

        public int Capacity { get; set; }

        public GameHierarchyEnum RequiredRole { get; set; }
    }
}
