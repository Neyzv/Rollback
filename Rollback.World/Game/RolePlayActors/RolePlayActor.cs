using Rollback.Protocol.Enums;
using Rollback.Protocol.Types;
using Rollback.World.Game.Criterion;
using Rollback.World.Game.Looks;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.World.Maps;
using Rollback.World.Handlers.Maps;

namespace Rollback.World.Game.RolePlayActors
{
    public abstract class RolePlayActor : WorldObject
    {
        private readonly CriterionExpression? _visibilityCriterion;

        public int Id { get; }

        public bool IsVisible
        {
            get;
            private set;
        }

        public virtual ActorLook Look { get; set; }

        public DirectionsEnum Direction { get; protected set; }

        public EntityDispositionInformations EntityDispositionInformations =>
            new(Cell.Id, (sbyte)Direction);

        public RolePlayActor(int id, MapInstance mapInstance, Cell cell, DirectionsEnum direction, ActorLook look,
            CriterionExpression? visibilityCriterion = default)
            : base(mapInstance, cell)
        {
            Id = id;
            MapInstance = mapInstance;
            Cell = cell;
            Direction = direction;
            Look = look;
            IsVisible = true;
            _visibilityCriterion = visibilityCriterion;
        }

        public void Refresh()
        {
            MapInstance?.Send(x => x.Account!.Character!.Id == Id || CanBeSee(x.Account.Character), MapHandler.SendGameRolePlayShowActorMessage, new object[] { this });
            MapInstance?.Send(x => x.Account!.Character!.Id != Id && !CanBeSee(x.Account.Character), MapHandler.SendGameContextRemoveElementMessage, new object[] { Id });
        }

        public void ChangeVisibility(bool visible)
        {
            IsVisible = visible;
            Refresh();
        }

        public void ChangeDirection(DirectionsEnum direction)
        {
            Direction = direction;
            MapInstance.Send(MapHandler.SendGameMapChangeOrientationMessage, new object[] { this });
        }

        protected virtual void Move(short[] keyMovements)
        {
            Cell = MapInstance.Map.Record.Cells[Cell.KeyMovementToCellId(keyMovements[^1])];
            Direction = Cell.KeyMovementToDirection(keyMovements[^1]);
        }

        public abstract GameRolePlayActorInformations GameRolePlayActorInformations(Character character);

        public bool CanBeSee(Character character) =>
            IsVisible && (_visibilityCriterion is null || _visibilityCriterion.Eval(character));
    }
}
