using Rollback.Protocol.Enums;
using Rollback.World.Game.Criterion;
using Rollback.World.Game.Looks;
using Rollback.World.Game.World.Maps;
using Rollback.World.Game.World.Maps.CellsZone.Shapes;
using Rollback.World.Game.World.Maps.PathFinding;
using Rollback.World.Handlers.Chat;
using Rollback.World.Handlers.Maps;

namespace Rollback.World.Game.RolePlayActors
{
    public abstract class ContextualActor : RolePlayActor
    {
        public ContextualActor(int id, MapInstance mapInstance, Cell cell, DirectionsEnum direction, ActorLook look, CriterionExpression? visibilityCriterion = default)
            : base(id, mapInstance, cell, direction, look, visibilityCriterion)
        {
        }

        public void AutoMove()
        {
            if (Cell.Point.GetAdjacentCells().Any(x => MapInstance.Map.Record.Cells[x.CellId].Walkable))
            {
                var possibleCells = new Lozenge(MapInstance.Map, Cell, 2, Direction).AffectedCells.Values.Where(x => x.Id != Cell.Id && x.Walkable).ToArray();
                if (possibleCells.Length is not 0)
                {
                    var destinationCell = possibleCells.ElementAt(Random.Shared.Next(possibleCells.Length - 1));
                    var path = PathFinder.Resolve(new[] { Cell.Id, destinationCell.Id }, MapInstance);

                    if (path.Cells.Length > 1)
                    {
                        Move(path.KeyMovements);
                        MapInstance!.Send(MapHandler.SendGameMapMovementMessage, new object[] { Id, path.KeyMovements });
                    }
                }
            }
        }

        public void Say(string message) =>
            ChatHandler.SendBasicChatServerMessage(this, DateTime.Now, message);
    }
}
