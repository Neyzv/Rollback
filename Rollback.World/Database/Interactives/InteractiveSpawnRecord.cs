using Rollback.Common.Logging;
using Rollback.Common.ORM;

namespace Rollback.World.Database.Interactives
{
    public static class InteractiveSpawnRelator
    {
        public const string GetSpawns = "SELECT * FROM interactives_spawns";
    }

    [Table("interactives_spawns")]
    public sealed record InteractiveSpawnRecord
    {
        public InteractiveSpawnRecord() =>
            (_skillsIdsCSV, SkillIds) = (string.Empty, Array.Empty<short>());

        [Key]
        public int Id { get; set; }

        public int MapId { get; set; }

        public short CellId { get; set; }

        public int ElementId { get; set; }

        public bool Animated { get; set; }

        private string _skillsIdsCSV;
        public string SkillsIdsCSV
        {
            get => _skillsIdsCSV;
            set
            {
                _skillsIdsCSV = value;

                if (!string.IsNullOrEmpty(value))
                    try
                    {
                        SkillIds = value.Split(',').Select(x => short.Parse(x)).ToArray();
                    }
                    catch
                    {
                        Logger.Instance.LogError(msg: $"Error while parsing SkillsIdsCSV of interactive spawn {Id}...");
                    }
            }
        }

        [Ignore]
        public short[] SkillIds { get; private set; }
    }
}
