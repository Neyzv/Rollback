using Rollback.Common.DesignPattern.Threading.Schedul;
using Rollback.Common.DesignPattern.Threading.Schedul.Callback;
using Rollback.World.Game.ScheduledExecutions.Types.Abstractions;
using Rollback.World.Network;

namespace Rollback.World.Game.ScheduledExecutions.Types
{
    public sealed class SaveWorldScheduledExecution : BaseScheduledExecution
    {
        private const string Id = "Save world";

        public override string Identifier =>
            Id;

        public override IExecution Start() =>
            Scheduler.Instance.ExecutePeriodically(WorldServer.Instance.SaveWorld)
                .WithTime(TimeSpan.FromMilliseconds(GeneralConfiguration.Instance.AutoSaveInterval));
    }
}
