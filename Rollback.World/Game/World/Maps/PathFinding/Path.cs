using Rollback.Protocol.Enums;

namespace Rollback.World.Game.World.Maps.PathFinding
{
    public sealed class Path
    {
        public Cell[] Cells { get; }

        public bool Success { get; }

        private short[]? _keyMovements;
        public short[] KeyMovements =>
            _keyMovements ??= BuildKeyMovements();

        public Path(Cell[] cells, bool success)
        {
            Cells = cells;
            Success = success;
        }

        private short[] BuildKeyMovements()
        {
            var keyMovements = new List<short>();

            var lastDirection = default(DirectionsEnum?);
            for (var i = 1; i < Cells.Length; i++)
            {
                var orientation = Cells[i - 1].Point.OrientationTo(Cells[i].Point);
                if (lastDirection != orientation)
                {
                    keyMovements.Add((short)((int)orientation << 12 | Cells[i - 1].Id & 4095));
                    lastDirection = orientation;
                }
            }

            if(keyMovements.Count > 0)
                keyMovements.Add((short)((keyMovements[^1] >> 12 & 7) << 12 | Cells[^1].Id & 4095));

            return keyMovements.ToArray();
        }
    }
}
