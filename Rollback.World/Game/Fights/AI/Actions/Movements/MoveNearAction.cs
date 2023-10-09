using Rollback.World.Game.World.Maps;
using Rollback.World.Game.World.Maps.PathFinding;

namespace Rollback.World.Game.Fights.AI.Actions.Movements
{
    internal class MoveNearAction : AIAction
    {
        private readonly Cell _targetedCell;

        public MoveNearAction(AIFighter fighter, Cell targetedCell) : base(fighter) =>
            _targetedCell = targetedCell;

        public override void Apply()
        {
            var path = PathFinder.Resolve(new[] { _fighter.Cell.Id, _targetedCell.Id }, _fighter.Team!.Fight.Map, _fighter);

            if (path.Cells.Length > 1)
                _fighter.Move(path.KeyMovements);
        }
    }
}
