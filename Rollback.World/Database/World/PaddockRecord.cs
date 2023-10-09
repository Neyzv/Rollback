using Rollback.Common.Logging;
using Rollback.Common.ORM;

namespace Rollback.World.Database.World
{
    public static class PaddockRelator
    {
        public const string GetAllPaddocks = "SELECT * FROM world_paddocks";
    }

    [Table("world_paddocks")]
    public sealed record PaddockRecord
    {
        [Key]
        public int MapId { get; set; }

        public int GuildId { get; set; }

        public short MaxOutdoorMount { get; set; }

        public short MaxItems { get; set; }

        public int Price { get; set; }

        private string _referenceCellIdsCSV = string.Empty;

        public string ReferenceCellIdsCSV
        {
            get => _referenceCellIdsCSV;
            set
            {
                _referenceCellIdsCSV = value;

                if (!string.IsNullOrWhiteSpace(_referenceCellIdsCSV))
                {
                    foreach(var cellIdStr in _referenceCellIdsCSV.Split(';'))
                        if(short.TryParse(cellIdStr, out var cellId))
                            ReferenceCellIds.Add(cellId);
                        else
                            Logger.Instance.LogError(msg: $"Can not parse {nameof(ReferenceCellIdsCSV)} of {nameof(PaddockRecord)} {MapId}...");
                }
            }
        }

        [Ignore]
        public List<short> ReferenceCellIds { get; set; } = new();
    }
}
