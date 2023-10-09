using Rollback.Common.ORM;

namespace Rollback.World.Database.Experiences
{
    public static class ExperienceRelator
    {
        public const string GetFloors = "SELECT * FROM experiences ORDER BY Level";
    }

    [Table("experiences")]
    public sealed record ExperienceRecord
    {
        [Key]
        public byte Level { get; set; }

        public long Characters { get; set; }

        public long Guilds { get; set; }

        public int Jobs { get; set; }

        public long Mounts { get; set; }

        public short Alignments { get; set; }
    }
}
