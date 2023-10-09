using Rollback.Protocol.Enums;

namespace Rollback.World.Game.World.Maps.CellsZone.Shapes
{
    public sealed class Cross : Zone
    {
        public bool OnlyPerpendicular { get; set; }

        public bool Diagonal { get; set; }

        public bool AllDirections { get; set; }

        public Cross(Map map, Cell centerCell, uint radius, DirectionsEnum direction, bool onlyPerpendicular, bool diagonal, bool alldirections) : base(map, centerCell, radius, direction)
        {
            OnlyPerpendicular = onlyPerpendicular;
            Diagonal = diagonal;
            AllDirections = alldirections;
        }

        protected override Dictionary<short, Cell> GetAffectedCells()
        {
            var result = new Dictionary<short, Cell>
            {
                [CenterCell.Id] = CenterCell
            };

            var disabledDirections = new List<DirectionsEnum>();
            if (OnlyPerpendicular)
            {
                switch (_direction)
                {
                    case DirectionsEnum.DIRECTION_SOUTH_EAST:
                    case DirectionsEnum.DIRECTION_NORTH_WEST:
                        {
                            disabledDirections.Add(DirectionsEnum.DIRECTION_SOUTH_EAST);
                            disabledDirections.Add(DirectionsEnum.DIRECTION_NORTH_WEST);
                            break;
                        }
                    case DirectionsEnum.DIRECTION_NORTH_EAST:
                    case DirectionsEnum.DIRECTION_SOUTH_WEST:
                        {
                            disabledDirections.Add(DirectionsEnum.DIRECTION_NORTH_EAST);
                            disabledDirections.Add(DirectionsEnum.DIRECTION_SOUTH_WEST);
                            break;
                        }
                    case DirectionsEnum.DIRECTION_SOUTH:
                    case DirectionsEnum.DIRECTION_NORTH:
                        {
                            disabledDirections.Add(DirectionsEnum.DIRECTION_SOUTH);
                            disabledDirections.Add(DirectionsEnum.DIRECTION_NORTH);
                            break;
                        }
                    case DirectionsEnum.DIRECTION_EAST:
                    case DirectionsEnum.DIRECTION_WEST:
                        {
                            disabledDirections.Add(DirectionsEnum.DIRECTION_EAST);
                            disabledDirections.Add(DirectionsEnum.DIRECTION_WEST);
                            break;
                        }
                }
            }

            var centerPoint = CenterCell.Point;

            for (var i = (int)Radius; i > 0; i--)
            {
                if (!Diagonal)
                {
                    if (!disabledDirections.Contains(DirectionsEnum.DIRECTION_SOUTH_EAST) && MapPoint.IsInMap(centerPoint.X + i, centerPoint.Y))
                    {
                        var cellId = MapPoint.CoordToCellId(centerPoint.X + i, centerPoint.Y);
                        result[cellId] = _map.Record.Cells[cellId];
                    }


                    if (!disabledDirections.Contains(DirectionsEnum.DIRECTION_NORTH_WEST) && MapPoint.IsInMap(centerPoint.X - i, centerPoint.Y))
                    {
                        var cellId = MapPoint.CoordToCellId(centerPoint.X - i, centerPoint.Y);
                        result[cellId] = _map.Record.Cells[cellId];
                    }

                    if (!disabledDirections.Contains(DirectionsEnum.DIRECTION_NORTH_EAST) && MapPoint.IsInMap(centerPoint.X, centerPoint.Y + i))
                    {
                        var cellId = MapPoint.CoordToCellId(centerPoint.X, centerPoint.Y + i);
                        result[cellId] = _map.Record.Cells[cellId];
                    }

                    if (!disabledDirections.Contains(DirectionsEnum.DIRECTION_SOUTH_WEST) && MapPoint.IsInMap(centerPoint.X, centerPoint.Y - i))
                    {
                        var cellId = MapPoint.CoordToCellId(centerPoint.X, centerPoint.Y - i);
                        result[cellId] = _map.Record.Cells[cellId];
                    }
                }

                if (Diagonal || AllDirections)
                {
                    if (!disabledDirections.Contains(DirectionsEnum.DIRECTION_SOUTH) && MapPoint.IsInMap(centerPoint.X + i, centerPoint.Y - i))
                    {
                        var cellId = MapPoint.CoordToCellId(centerPoint.X + i, centerPoint.Y - i);
                        result[cellId] = _map.Record.Cells[cellId];
                    }

                    if (!disabledDirections.Contains(DirectionsEnum.DIRECTION_NORTH) && MapPoint.IsInMap(centerPoint.X - i, centerPoint.Y + i))
                    {
                        var cellId = MapPoint.CoordToCellId(centerPoint.X - i, centerPoint.Y + i);
                        result[cellId] = _map.Record.Cells[cellId];
                    }

                    if (!disabledDirections.Contains(DirectionsEnum.DIRECTION_EAST) && MapPoint.IsInMap(centerPoint.X + i, centerPoint.Y + i))
                    {
                        var cellId = MapPoint.CoordToCellId(centerPoint.X + i, centerPoint.Y + i);
                        result[cellId] = _map.Record.Cells[cellId];
                    }

                    if (!disabledDirections.Contains(DirectionsEnum.DIRECTION_WEST) && MapPoint.IsInMap(centerPoint.X - i, centerPoint.Y - i))
                    {
                        var cellId = MapPoint.CoordToCellId(centerPoint.X - i, centerPoint.Y - i);
                        result[cellId] = _map.Record.Cells[cellId];
                    }
                }
            }

            return result;
        }
    }
}
