using Rollback.Protocol.Enums;

namespace Rollback.World.Game.World.Maps.CellsZone
{
    public abstract class Zone
    {
        protected Map _map;
        protected DirectionsEnum _direction;

        public Cell CenterCell { get; }

        public uint Radius { get; }

        private Dictionary<short, Cell>? _affectedCells;
        public Dictionary<short, Cell> AffectedCells =>
            _affectedCells ??= GetAffectedCells();

        public Zone(Map map, Cell centerCell, uint radius, DirectionsEnum direction)
        {
            _map = map;
            CenterCell = centerCell;
            Radius = radius;
            _direction = direction;
        }

        protected abstract Dictionary<short, Cell> GetAffectedCells();
    }
}
