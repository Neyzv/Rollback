using System.Diagnostics.CodeAnalysis;
using Rollback.Common.Initialization;
using Rollback.Common.Logging;
using Rollback.Protocol.Enums;

namespace Rollback.World.Game.World.Maps
{
    public sealed class MapPoint
    {
        public const uint MapWidth = 14;
        public const uint MapHeight = 20;
        private const byte MinPointsForAPolygon = 3;

        public const uint MapSize = MapWidth * MapHeight * 2;

        private static readonly MapPoint[] _orthogonalGridReference;

        [NotNull]
        public static MapPoint? CenterPoint { get; private set; }

        public short CellId { get; }

        public int X { get; }

        public int Y { get; }

        static MapPoint() =>
            _orthogonalGridReference = new MapPoint[MapSize];

        private MapPoint(int x, int y)
        {
            X = x;
            Y = y;
            CellId = CoordToCellId(X, Y);
        }

        [Initializable(InitializationPriority.DatasManager, "Orthogonal grid")]
        public static void Initialize()
        {
            var posX = 0;
            var posY = 0;
            var cellCount = 0;

            for (var x = 0; x < MapHeight; x++)
            {
                for (var y = 0; y < MapWidth; y++)
                    _orthogonalGridReference[cellCount++] = new(posX + y, posY + y);

                posX++;

                for (var y = 0; y < MapWidth; y++)
                    _orthogonalGridReference[cellCount++] = new(posX + y, posY + y);

                posY--;
            }

            var diff = (int)MapWidth - (int)MapHeight;
            var absDiff = Math.Abs(diff);
            var yCenter = (int)Math.Sqrt(absDiff);

            CenterPoint = FromCoords((int)Math.Sqrt(Math.Pow(MapWidth + absDiff, 2) + Math.Pow(MapHeight + absDiff, 2)) / 2, diff > 0 ? yCenter : -yCenter);
        }

        public static short CoordToCellId(int x, int y) =>
            (short)((x - y) * MapWidth + y + (x - y) / 2);

        public static MapPoint FromCellId(short cellId)
        {
            if (cellId < 0 || cellId > MapSize)
                Logger.Instance.LogError(msg: $"Cell identifier out of bounds {cellId}...");

            return _orthogonalGridReference[cellId];
        }

        public static MapPoint FromCoords(int x, int y) =>
            FromCellId(CoordToCellId(x, y));

        public static bool IsInMap(int x, int y) =>
            x + y >= 0 && x - y >= 0 && x - y < MapHeight * 2 && x + y < MapWidth * 2;

        public uint ManhattanDistanceTo(MapPoint point) =>
            (uint)(Math.Abs(X - point.X) + Math.Abs(Y - point.Y));

        public uint EuclidianDistanceTo(MapPoint point) =>
            (uint)Math.Sqrt(Math.Pow(X - point.X, 2) + Math.Pow(Y - point.Y, 2));

        public bool IsAdjacentTo(MapPoint point) =>
            ManhattanDistanceTo(point) is 1;

        public bool IsOnSameLine(MapPoint point) =>
            X == point.X || Y == point.Y;

        public IEnumerable<MapPoint> GetAdjacentCells(Predicate<short>? p = default, bool diagonal = false)
        {
            if (IsInMap(X + 1, Y))
            {
                var cell = new MapPoint(X + 1, Y);
                if (p is null || p(cell.CellId))
                    yield return cell;
            }

            if (IsInMap(X, Y - 1))
            {
                var cell = new MapPoint(X, Y - 1);
                if (p is null || p(cell.CellId))
                    yield return cell;
            }

            if (IsInMap(X, Y + 1))
            {
                var cell = new MapPoint(X, Y + 1);
                if (p is null || p(cell.CellId))
                    yield return cell;
            }

            if (IsInMap(X - 1, Y))
            {
                var cell = new MapPoint(X - 1, Y);
                if (p is null || p(cell.CellId))
                    yield return cell;
            }

            if (diagonal)
            {
                var south = new MapPoint(X + 1, Y - 1);
                if (IsInMap(south.X, south.Y) && (p is null || p(south.CellId)))
                    yield return south;

                var west = new MapPoint(X - 1, Y - 1);
                if (IsInMap(west.X, west.Y) && (p is null || p(west.CellId)))
                    yield return west;

                var north = new MapPoint(X - 1, Y + 1);
                if (IsInMap(north.X, north.Y) && (p is null || p(north.CellId)))
                    yield return north;

                var east = new MapPoint(X + 1, Y + 1);
                if (IsInMap(east.X, east.Y) && (p is null || p(east.CellId)))
                    yield return east;
            }
        }

        public DirectionsEnum OrientationTo(MapPoint point, bool diagonal = true)
        {
            int dx = point.X - X;
            int dy = Y - point.Y;

            double distance = Math.Sqrt(dx * dx + dy * dy);
            double angleInRadians = Math.Acos(dx / distance);

            double angleInDegrees = angleInRadians * 180 / Math.PI;
            double transformedAngle = angleInDegrees * (point.Y > Y ? (-1) : 1);

            double orientation = !diagonal ? Math.Round(transformedAngle / 90) * 2 + 1 : Math.Round(transformedAngle / 45) + 1;

            if (orientation < 0)
                orientation += 8;

            return (DirectionsEnum)(uint)orientation;
        }

        public MapPoint? GetCellInDirection(DirectionsEnum direction, short step)
        {
            var point = direction switch
            {
                DirectionsEnum.DIRECTION_EAST => new(X + step, Y + step),
                DirectionsEnum.DIRECTION_SOUTH_EAST => new(X + step, Y),
                DirectionsEnum.DIRECTION_SOUTH => new(X + step, Y - step),
                DirectionsEnum.DIRECTION_SOUTH_WEST => new(X, Y - step),
                DirectionsEnum.DIRECTION_WEST => new(X - step, Y - step),
                DirectionsEnum.DIRECTION_NORTH_WEST => new(X - step, Y),
                DirectionsEnum.DIRECTION_NORTH => new(X - step, Y + step),
                DirectionsEnum.DIRECTION_NORTH_EAST => new(X, Y + step),
                _ => default(MapPoint?)
            };

            return point is not null && IsInMap(point.X, point.Y) ? point : default;
        }

        public bool IsInside(IList<MapPoint> points)
        {
            var result = false;

            if (points.Count >= MinPointsForAPolygon)
            {
                points = points.OrderBy(p => p.X).ToList();
            
                for (int i = 0, j = points.Count - 1; i < points.Count; j = i++)
                {
                    if (points[i].Y > Y != points[j].Y > Y &&
                        X < (points[j].X - points[i].X) * (Y - points[i].Y) / (points[j].Y - points[i].Y) + points[i].X)
                        result = !result;
                }
            }

            return result;
        }
    }
}
