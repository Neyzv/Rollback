using System.IO.Compression;
using Rollback.Common.Logging;
using Rollback.Protocol.Enums;

namespace Rollback.World.Game.World.Maps
{
    public sealed class Cell
    {
        public const int StructSize = 7;
        public const byte AdjacentCellsCount = 8;

        public short Id { get; set; }

        public int Floor { get; set; }

        public byte LosMov { get; set; }

        public byte Speed { get; set; }

        public byte MapChangeData { get; set; }

        public bool LineOfSight =>
            (LosMov & 2) >> 1 == 1;

        public bool Walkable =>
            (LosMov & 1) == 1 && !NonWalkableDuringFight;

        public bool NonWalkableDuringFight =>
            (LosMov & 3) >> 2 == 1;

        public bool Red =>
            (LosMov & 4) >> 3 == 1;

        public bool Blue =>
            (LosMov & 5) >> 4 == 1;

        private MapPoint? _point;
        public MapPoint Point =>
            _point ??= MapPoint.FromCellId(Id);

        private static Cell Deserialize(byte[] data, int index = 0) =>
            new()
            {
                Id = (short)(data[index] << 8 | data[index + 1]),
                Floor = (short)(data[index + 2] << 8 | data[index + 3]),
                LosMov = data[index + 4],
                MapChangeData = data[index + 5],
                Speed = data[index + 6],
            };

        public static Cell[] UnCompressCells(byte[] compressedCells)
        {
            Cell[] cells = Array.Empty<Cell>();
            var input = new MemoryStream(compressedCells);
            var output = new MemoryStream();

            try
            {
                using (var zinput = new GZipStream(input, CompressionMode.Decompress, true))
                    zinput.CopyTo(output);

                byte[] uncompressedCells = output.ToArray();
                cells = new Cell[uncompressedCells.Length / StructSize];

                for (int i = 0, j = 0; i < uncompressedCells.Length; i += StructSize, j++)
                    cells[j] = Deserialize(uncompressedCells, i);
            }
            catch
            {
                Logger.Instance.LogError(msg: "Error while loading a map incorrect cells compression...");
            }

            return cells;
        }

        public static short KeyMovementToCellId(short keyMovement) =>
            (short)(keyMovement & 4095);

        public static DirectionsEnum KeyMovementToDirection(short keyMovement) =>
            (DirectionsEnum)(keyMovement >> 12 & 7);

        public static bool CellIdValid(short cellId) =>
            cellId > 0 && cellId < MapPoint.MapSize;
    }
}
