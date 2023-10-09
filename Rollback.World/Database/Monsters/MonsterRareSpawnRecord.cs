using Rollback.Common.Logging;
using Rollback.Common.ORM;

namespace Rollback.World.Database.Monsters
{
    public static class MonsterRareSpawnRelator
    {
        public const string QueryAll = "SELECT * FROM monsters_rare_spawns";
    }

    [Table("monsters_rare_spawns")]
    public sealed record MonsterRareSpawnRecord
    {
        [Key]
        public int Id { get; set; }

        private string? _mapIdsCSV;
        public string? MapIdsCSV
        {
            get => _mapIdsCSV;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    MapIds.Clear();
                    _mapIdsCSV = value;

                    foreach (var mapId in value.Split(';'))
                    {
                        if (int.TryParse(mapId, out var id))
                            MapIds.Add(id);
                        else
                            Logger.Instance.LogError(msg: $"Error while parsing {nameof(MapIdsCSV)} of {nameof(MonsterRareSpawnRecord)} {Id}...");
                    }
                }
            }
        }

        [Ignore]
        public List<int> MapIds { get; private set; } = new();


        private string? _subAreaIds;
        public string? SubAreaIdsCSV
        {
            get => _subAreaIds;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    SubAreaIds.Clear();
                    _subAreaIds = value;

                    foreach (var subAreaId in value.Split(';'))
                    {
                        if (short.TryParse(subAreaId, out var id))
                            SubAreaIds.Add(id);
                        else
                            Logger.Instance.LogError(msg: $"Error while parsing {nameof(SubAreaIdsCSV)} of {nameof(MonsterRareSpawnRecord)} {Id}...");
                    }
                }
            }
        }

        [Ignore]
        public List<short> SubAreaIds { get; set; } = new();

        public short MonsterId { get; set; }

        public sbyte MinGrade { get; set; }

        public sbyte MaxGrade { get; set; }

        public short MinRespawnMinute { get; set; }

        public short MaxRespawnMinute { get; set; }
    }
}
