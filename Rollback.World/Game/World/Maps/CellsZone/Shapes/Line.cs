using Rollback.Protocol.Enums;
using Rollback.World.Extensions;

namespace Rollback.World.Game.World.Maps.CellsZone.Shapes
{
    public sealed class Line : Zone
    {
        public bool OpposedDirection { get; set; }

        public Line(Map map, Cell centerCell, uint radius, DirectionsEnum direction, bool opposedDirection) : base(map, centerCell, radius, direction) =>
            OpposedDirection = opposedDirection;

        protected override Dictionary<short, Cell> GetAffectedCells()
        {
            if (OpposedDirection)
                _direction = _direction.GetOpposedDirection();

            var result = new Dictionary<short, Cell>();

            for (int i = 0; i <= Radius; i++)
            {
                switch (_direction)
                {
                    case DirectionsEnum.DIRECTION_WEST:
                        if (MapPoint.IsInMap(CenterCell.Point.X - i, CenterCell.Point.Y - i))
                        {
                            var cellId = MapPoint.CoordToCellId(CenterCell.Point.X - i, CenterCell.Point.Y - i);
                            result[cellId] = _map.Record.Cells[cellId];
                        }
                        break;
                    case DirectionsEnum.DIRECTION_NORTH:
                        if (MapPoint.IsInMap(CenterCell.Point.X - i, CenterCell.Point.Y + i))
                        {
                            var cellId = MapPoint.CoordToCellId(CenterCell.Point.X - i, CenterCell.Point.Y + i);
                            result[cellId] = _map.Record.Cells[cellId];
                        }
                        break;
                    case DirectionsEnum.DIRECTION_EAST:
                        if (MapPoint.IsInMap(CenterCell.Point.X + i, CenterCell.Point.Y + i))
                        {
                            var cellId = MapPoint.CoordToCellId(CenterCell.Point.X + i, CenterCell.Point.Y + i);
                            result[cellId] = _map.Record.Cells[cellId];
                        }
                        break;
                    case DirectionsEnum.DIRECTION_SOUTH:
                        if (MapPoint.IsInMap(CenterCell.Point.X + i, CenterCell.Point.Y - i))
                        {
                            var cellId = MapPoint.CoordToCellId(CenterCell.Point.X + i, CenterCell.Point.Y - i);
                            result[cellId] = _map.Record.Cells[cellId];
                        }
                        break;
                    case DirectionsEnum.DIRECTION_NORTH_WEST:
                        if (MapPoint.IsInMap(CenterCell.Point.X - i, CenterCell.Point.Y))
                        {
                            var cellId = MapPoint.CoordToCellId(CenterCell.Point.X - i, CenterCell.Point.Y);
                            result[cellId] = _map.Record.Cells[cellId];
                        }
                        break;
                    case DirectionsEnum.DIRECTION_SOUTH_WEST:
                        if (MapPoint.IsInMap(CenterCell.Point.X, CenterCell.Point.Y - i))
                        {
                            var cellId = MapPoint.CoordToCellId(CenterCell.Point.X, CenterCell.Point.Y - i);
                            result[cellId] = _map.Record.Cells[cellId];
                        }
                        break;
                    case DirectionsEnum.DIRECTION_SOUTH_EAST:
                        if (MapPoint.IsInMap(CenterCell.Point.X + i, CenterCell.Point.Y))
                        {
                            var cellId = MapPoint.CoordToCellId(CenterCell.Point.X + i, CenterCell.Point.Y);
                            result[cellId] = _map.Record.Cells[cellId];
                        }
                        break;
                    case DirectionsEnum.DIRECTION_NORTH_EAST:
                        if (MapPoint.IsInMap(CenterCell.Point.X, CenterCell.Point.Y + i))
                        {
                            var cellId = MapPoint.CoordToCellId(CenterCell.Point.X, CenterCell.Point.Y + i);
                            result[cellId] = _map.Record.Cells[cellId];
                        }
                        break;
                }
            }

            return result;
        }
    }
}
