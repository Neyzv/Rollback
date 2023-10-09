using Rollback.Common.DesignPattern.Attributes;
using Rollback.Common.DesignPattern.Threading.Schedul;
using Rollback.Protocol.Enums;
using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;
using Rollback.World.Database.World;
using Rollback.World.Extensions;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.World.Maps.Triggers.Actions
{
    [Identifier("Animate")]
    public sealed class AnimateTriggerAction : CellTrigger
    {
        private int? _mapId;
        public int MapId =>
            _mapId ??= GetParameterValue<int>(0);

        private int? _interactiveId;
        public int InteractiveId =>
            _interactiveId ??= GetParameterValue<int>(1);

        private string? _obstaclesStr;
        public string? ObstaclesStr =>
            _obstaclesStr ??= GetParameterValue<string>(2);

        private HashSet<short>? _obstacleCellIds;
        public HashSet<short> ObstacleCellIds =>
            _obstacleCellIds ??= ParseObstacleCellIds();

        public AnimateTriggerAction(WorldCellsTriggersRecord record) : base(record) { }

        private HashSet<short> ParseObstacleCellIds()
        {
            var result = new HashSet<short>();

            if (!string.IsNullOrWhiteSpace(ObstaclesStr))
            {
                foreach (var obstacleCellIdStr in ObstaclesStr.Split(','))
                    if (short.TryParse(obstacleCellIdStr, out var obstacleCellId))
                        result.Add(obstacleCellId);
            }

            return result;
        }

        private void CloseAnimatedInteractive()
        {
            if (WorldManager.Instance.GetMapById(MapId) is { } map)
            {
                foreach (var instance in map.GetInstances())
                    if (instance.GetInteractiveById(InteractiveId) is { } interactive)
                    {
                        interactive.ChangeState(InteractiveState.Normal);

                        var obstaclesToRefresh = new List<MapObstacle>();
                        foreach (var obstacleCellId in ObstacleCellIds)
                            if (instance.GetMapObstacleByCellId(obstacleCellId) is { } obstacle)
                            {
                                obstacle.state = (sbyte)MapObstacleStateEnum.OBSTACLE_CLOSED;
                                obstaclesToRefresh.Add(obstacle);
                            }

                        foreach (var character in instance.GetActors<Character>(x => ObstacleCellIds.Contains(x.Cell.Id)))
                            character.Teleport(instance, instance.GetRandomAdjacentFreeCell(character.Cell.Point, true)!.Id);

                        obstaclesToRefresh.Refresh(instance);
                    }
            }
        }

        public override void Trigger(Character character)
        {
            if (WorldManager.Instance.GetMapById(MapId) is { } map)
            {
                foreach (var instance in map.GetInstances())
                    if (instance.GetInteractiveById(InteractiveId) is { } interactive &&
                        interactive.State is not InteractiveState.Activated)
                    {
                        interactive.ChangeState(InteractiveState.Activated);

                        var obstaclesToRefresh = new List<MapObstacle>();
                        foreach (var obstacleCellId in ObstacleCellIds)
                            if (instance.GetMapObstacleByCellId(obstacleCellId) is { } obstacle)
                            {
                                obstacle.state = (sbyte)MapObstacleStateEnum.OBSTACLE_OPENED;
                                obstaclesToRefresh.Add(obstacle);
                            }

                        obstaclesToRefresh.Refresh(instance);
                    }

                Scheduler.Instance.ExecuteDelayed(CloseAnimatedInteractive)
                    .WithTime(TimeSpan.FromMilliseconds(WorldManager.InteractiveAnimateDurationTime));
            }
        }
    }
}
