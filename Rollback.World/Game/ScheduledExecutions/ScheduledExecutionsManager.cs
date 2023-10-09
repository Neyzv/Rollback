using System.Collections.Concurrent;
using Rollback.Common.DesignPattern.Assemblies;
using Rollback.Common.DesignPattern.Instance;
using Rollback.Common.DesignPattern.Threading.Schedul;
using Rollback.Common.DesignPattern.Threading.Schedul.Callback;
using Rollback.Common.Initialization;
using Rollback.World.Game.ScheduledExecutions.Types.Abstractions;

namespace Rollback.World.Game.ScheduledExecutions
{
    public sealed class ScheduledExecutionsManager : Singleton<ScheduledExecutionsManager>
    {
        private readonly ConcurrentDictionary<string, IExecution> _executions;

        public ScheduledExecutionsManager() =>
            _executions = new ConcurrentDictionary<string, IExecution>();

        [Initializable(InitializationPriority.ScheduledTasks, "Scheduled executions")]
        public void Initialize()
        {
            var baseExecutionType = typeof(BaseScheduledExecution);

            foreach (var type in from assembly in AssemblyManager.Instance.Assemblies
                                 from type in assembly.GetTypes()
                                 where type.IsSubclassOf(baseExecutionType) && !type.IsAbstract
                                 select type)
            {
                var ctor = type.GetConstructor(Array.Empty<Type>());

                if (ctor is not null)
                    TryStartExecution((BaseScheduledExecution)ctor.Invoke(Array.Empty<object>()));
            }
        }

        public void TryStartExecution(BaseScheduledExecution execution) =>
            _executions.TryAdd(execution.Identifier, execution.Start());

        public void TryStopExecution(BaseScheduledExecution execution)
        {
            _executions.TryRemove(execution.Identifier, out var exec);

            if (exec is not null)
                Scheduler.Instance.CancelExecutionPeriodically(exec);
        }
    }
}
