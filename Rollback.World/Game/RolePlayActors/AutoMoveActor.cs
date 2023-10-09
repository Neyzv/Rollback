using Rollback.Protocol.Enums;
using Rollback.World.Game.Criterion;
using Rollback.World.Game.Looks;
using Rollback.World.Game.World.Maps;

namespace Rollback.World.Game.RolePlayActors
{
    public abstract class AutoMoveActor : ContextualActor
    {
        private Timer? _timer;
        private Timer? _timerDisabler;

        protected AutoMoveActor(int id, MapInstance mapInstance, Cell cell, DirectionsEnum direction, ActorLook look,
            CriterionExpression? visibilityCriterion = null)
            : base(id, mapInstance, cell, direction, look, visibilityCriterion) { }

        private void AutoMoveGroup(object? sender)
        {
            _timer?.Dispose();
            _timer = default;

            AutoMove();

            EnableAutoMove();
        }

        public void EnableAutoMove()
        {
            if (_timer is null)
                _timer = new(new(AutoMoveGroup), default, Random.Shared.Next(GeneralConfiguration.Instance.MinAutoMoveInterval,
                    GeneralConfiguration.Instance.MaxAutoMoveInterval), Timeout.Infinite);
            else
                _timerDisabler?.Dispose();
        }

        private void StopAutoMoveGroup(object? sender)
        {
            _timer?.Dispose();
            _timerDisabler?.Dispose();
        }

        public void DisableAutoMove() =>
            _timerDisabler = new(new(StopAutoMoveGroup), default, GeneralConfiguration.Instance.AutoMoveDisableInterval, Timeout.Infinite);
    }
}
