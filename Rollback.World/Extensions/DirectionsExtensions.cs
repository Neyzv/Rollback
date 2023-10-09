using Rollback.Protocol.Enums;

namespace Rollback.World.Extensions
{
    public static class DirectionsExtensions
    {
        public static DirectionsEnum GetOpposedDirection(this DirectionsEnum direction) =>
            (DirectionsEnum)(((int)direction + 4) % 8);

        public static bool IsDiagonal(this DirectionsEnum direction) =>
            ((int)direction) % 2 == 0;

        public static DirectionsEnum[] GetDiagonalDecomposition(this DirectionsEnum direction) =>
            direction switch
            {
                DirectionsEnum.DIRECTION_EAST => new[] { DirectionsEnum.DIRECTION_NORTH_EAST, DirectionsEnum.DIRECTION_SOUTH_EAST },
                DirectionsEnum.DIRECTION_WEST => new[] { DirectionsEnum.DIRECTION_NORTH_WEST, DirectionsEnum.DIRECTION_SOUTH_WEST },
                DirectionsEnum.DIRECTION_SOUTH => new[] { DirectionsEnum.DIRECTION_SOUTH_EAST, DirectionsEnum.DIRECTION_SOUTH_WEST },
                DirectionsEnum.DIRECTION_NORTH => new[] { DirectionsEnum.DIRECTION_NORTH_WEST, DirectionsEnum.DIRECTION_NORTH_EAST },
                _ => new[] { direction }
            };
    }
}
