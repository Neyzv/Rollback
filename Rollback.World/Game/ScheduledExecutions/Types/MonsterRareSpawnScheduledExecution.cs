using Rollback.Common.DesignPattern.Threading.Schedul;
using Rollback.Common.DesignPattern.Threading.Schedul.Callback;
using Rollback.World.Game.RolePlayActors.Monsters;
using Rollback.World.Game.ScheduledExecutions.Types.Abstractions;

namespace Rollback.World.Game.ScheduledExecutions.Types
{
    public sealed class MonsterRareSpawnScheduledExecution : BaseScheduledExecution
    {
        private const string Id = "Monster rare spawn";

        public override string Identifier =>
            Id;

        public override IExecution Start() =>
            Scheduler.Instance.ExecutePeriodically(MonsterManager.Instance.CheckMonstersRareSpawnsAvailability)
                .WithTime(TimeSpan.FromMilliseconds(MonsterConfig.Instance.MonsterRareSpawnCheckInterval));
    }
}
