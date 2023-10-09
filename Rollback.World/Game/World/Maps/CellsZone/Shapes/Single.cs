using Rollback.Protocol.Enums;

namespace Rollback.World.Game.World.Maps.CellsZone.Shapes
{
    public sealed class Single : Zone
    {
        public Single(Map map, Cell centerCell, uint radius, DirectionsEnum direction) : base(map, centerCell, radius, direction) { }

        protected override Dictionary<short, Cell> GetAffectedCells() =>
            new() { [CenterCell.Id] = CenterCell };
    }
}
