namespace Rollback.World.Game.World.Maps.PathFinding
{
    public sealed class CellInformation
    {
        public Cell Cell { get; set; }

        public uint CostToReach { get; private set; }

        public uint DistanceToEnd { get; }

        public uint CostDistance =>
            DistanceToEnd + CostToReach;

        public CellInformation? Parent { get; private set; }

        public CellInformation(Cell cell, MapPoint endPoint)
        {
            Cell = cell;
            DistanceToEnd = endPoint.EuclidianDistanceTo(cell.Point);
        }

        public void SetParent(CellInformation parent)
        {
            Parent = parent;
            CostToReach = parent.CostToReach + 1;
        }
    }
}
