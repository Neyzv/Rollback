using System.Diagnostics.CodeAnalysis;
using Rollback.Common.Logging;
using Rollback.Common.ORM;
using Rollback.World.Game.World.Maps;

namespace Rollback.World.Database.World
{
    public static class MapRelator
    {
        public const string GetMaps = "SELECT * FROM world_maps";
    }

    [Table("world_maps")]
    public sealed record MapRecord
    {
        public MapRecord() =>
            (_defendersCellsCSV, DefendersCells, _challengersCellsCSV, ChallengersCells, _compressedCells) = (string.Empty, Array.Empty<short>(), string.Empty, Array.Empty<short>(), Array.Empty<byte>());

        [Key]
        public int Id { get; set; }

        public short SubAreaId { get; set; }

        public int TopNeighbourId { get; set; }

        public short? TopNeighbourCellId { get; set; }

        public int BottomNeighbourId { get; set; }

        public short? BottomNeighbourCellId { get; set; }

        public int LeftNeighbourId { get; set; }

        public short? LeftNeighbourCellId { get; set; }

        public int RightNeighbourId { get; set; }

        public short? RightNeighbourCellId { get; set; }

        public sbyte X { get; set; }

        public sbyte Y { get; set; }

        private string _defendersCellsCSV;
        public string DefendersCellsCSV
        {
            get => _defendersCellsCSV;
            set
            {
                _defendersCellsCSV = value;

                if (!string.IsNullOrEmpty(_defendersCellsCSV))
                    try
                    {
                        DefendersCells = value.Split(',').Select(x => short.Parse(x)).ToArray();
                    }
                    catch
                    {
                        Logger.Instance.LogError(default, $"Can not parse defenders cells of map {Id}...");
                    }
            }
        }

        [Ignore]
        public short[] DefendersCells { get; private set; }

        private string _challengersCellsCSV;
        public string ChallengersCellsCSV
        {
            get => _challengersCellsCSV;
            set
            {
                _challengersCellsCSV = value;

                if (!string.IsNullOrEmpty(_challengersCellsCSV))
                    try
                    {
                        ChallengersCells = value.Split(',').Select(x => short.Parse(x)).ToArray();
                    }
                    catch
                    {
                        Logger.Instance.LogError(default, $"Can not parse defenders cells of map {Id}...");
                    }
            }
        }

        [Ignore]
        public short[] ChallengersCells { get; private set; }

        private byte[] _compressedCells;
        public byte[] CompressedCells
        {
            get => _compressedCells;
            set
            {
                _compressedCells = value;
                Cells = Cell.UnCompressCells(value);
            }
        }

        [Ignore, NotNull]
        public Cell[]? Cells { get; private set; }

        public bool SpawnDisabled { get; set; }
    }
}
