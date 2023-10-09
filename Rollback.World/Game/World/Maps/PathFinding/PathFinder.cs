using Rollback.Protocol.Enums;
using Rollback.World.Game.Fights;
using Rollback.World.Game.RolePlayActors;

namespace Rollback.World.Game.World.Maps.PathFinding
{
    public static class PathFinder
    {
        private static List<CellInformation> GetAdjacentAvailableCells(MapInstance map, CellInformation cell, MapPoint end, FightActor? fighter,
            bool allDirections, bool dodgeActors) =>
            cell.Cell.Point.GetAdjacentCells(diagonal: allDirections).Select(x => new CellInformation(map.Map.Record.Cells[x.CellId], end))
                .Where(x => fighter is null ?
                    ((x.Cell.Walkable || (map.GetMapObstacleByCellId(x.Cell.Id) is { } obstacle && obstacle.state is (sbyte)MapObstacleStateEnum.OBSTACLE_OPENED))
                    && (!dodgeActors || x.Cell.Id == end.CellId || map.GetActor<RolePlayActor>(y => y.Cell.Id == x.Cell.Id) is null)) :
                    ((x.Cell.Walkable || !x.Cell.NonWalkableDuringFight) && (end.CellId == x.Cell.Id
                    || (fighter.Team!.Fight.GetAllFighters<FightActor>(y => y.Alive && y.Cell.Id == x.Cell.Id).Length is 0 &&
                    (!dodgeActors || cell.Cell.Point.GetAdjacentCells().All(z => fighter.Team!.OpposedTeam!.GetFighter<FightActor>(y => y.Alive && y.Cell.Id == z.CellId) is null)))))).ToList();

        private static Cell[] FindShortestPath(MapInstance map, CellInformation start, CellInformation end, FightActor? fighter,
            bool allDirections, bool dodgeActors)
        {
            var activeCells = new Dictionary<short, CellInformation>();
            var visitedCells = new Dictionary<short, CellInformation>();

            activeCells[start.Cell.Id] = start;

            var cellToCheck = start;
            while (activeCells.Count is not 0 && cellToCheck.Cell.Id != end.Cell.Id)
            {
                cellToCheck = activeCells.Values.OrderBy(x => x.CostDistance).First();

                visitedCells[cellToCheck.Cell.Id] = cellToCheck;
                activeCells.Remove(cellToCheck.Cell.Id);

                foreach (var cellInfo in GetAdjacentAvailableCells(map, cellToCheck, end.Cell.Point, fighter, allDirections, dodgeActors).Where(x => !visitedCells.ContainsKey(x.Cell.Id)))
                {
                    cellInfo.SetParent(cellToCheck);

                    if (activeCells.ContainsKey(cellInfo.Cell.Id))
                    {
                        if (activeCells[cellInfo.Cell.Id].CostDistance > cellInfo.CostDistance)
                        {
                            activeCells.Remove(activeCells[cellInfo.Cell.Id].Cell.Id);
                            activeCells[cellInfo.Cell.Id] = cellInfo;
                        }
                    }
                    else
                        activeCells[cellInfo.Cell.Id] = cellInfo;
                }
            }

            //BUILD PATH
            var path = new List<Cell>();
            while (cellToCheck is not null)
            {
                path.Add(cellToCheck.Cell);
                cellToCheck = cellToCheck.Parent;
            }

            path.Reverse();

            return path.ToArray();
        }

        public static Path Resolve(short[] keyCellIds, MapInstance map, FightActor? fighter = default, bool allDirections = false, bool dodgeActors = false)
        {
            if (keyCellIds.Length < 2)
                throw new Exception("Can not resolve a path who doesn't contain a start and an end...");

            var pathCells = new List<Cell>();
            for (var i = 1; i < keyCellIds.Length; i++)
            {
                if (keyCellIds[i - 1] > map.Map.Record.Cells.Length || keyCellIds[i] > map.Map.Record.Cells.Length)
                    throw new Exception("Can not identified start or end cell...");

                var startCell = map.Map.Record.Cells[keyCellIds[i - 1]];
                var endCell = map.Map.Record.Cells[keyCellIds[i]];

                var subPathCells = FindShortestPath(map, new(startCell, endCell.Point), new(endCell, endCell.Point), fighter, allDirections, dodgeActors);

                pathCells.AddRange(i is not 1 ? subPathCells[1..] : subPathCells);

                if (subPathCells.Last().Id != endCell.Id)
                    break;
            }

            return new Path(pathCells.ToArray(), pathCells.LastOrDefault()?.Id == keyCellIds[^1]);
        }
    }
}
