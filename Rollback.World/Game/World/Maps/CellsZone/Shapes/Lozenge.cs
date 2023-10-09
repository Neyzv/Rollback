using Rollback.Protocol.Enums;

namespace Rollback.World.Game.World.Maps.CellsZone.Shapes
{
    public sealed class Lozenge : Zone
    {
        public Lozenge(Map map, Cell centerCell, uint radius, DirectionsEnum direction) : base(map, centerCell, radius, direction) { }

        protected override Dictionary<short, Cell> GetAffectedCells()
        {
            var result = new Dictionary<short, Cell>();

            if (Radius is 0)
                result[CenterCell.Id] = CenterCell;
            else if (Radius > 0)
            {
                int x = (int)(CenterCell.Point.X - Radius);
                int y;
                int i = 0;
                int j = 1;

                while (x <= CenterCell.Point.X + Radius)
                {
                    y = -i;

                    while (y <= i)
                    {
                        if (MapPoint.IsInMap(x, y + CenterCell.Point.Y))
                        {
                            var cellId = MapPoint.CoordToCellId(x, y + CenterCell.Point.Y);
                            result[cellId] = _map.Record.Cells[cellId];
                        }
                        y++;
                    }

                    if (i == Radius)
                    {
                        j = -j;
                    }

                    i += j;
                    x++;
                }
            }

            return result;
        }
    }
}
