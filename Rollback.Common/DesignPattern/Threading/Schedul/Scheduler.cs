using System.Collections.Concurrent;
using Rollback.Common.DesignPattern.Collections;
using Rollback.Common.DesignPattern.Instance;
using Rollback.Common.DesignPattern.Threading.Schedul.Callback;
using Rollback.Common.Extensions;
using Rollback.Common.Initialization;

namespace Rollback.Common.DesignPattern.Threading.Schedul
{
    public sealed class Scheduler : Singleton<Scheduler>
    {
        private readonly SynchronizedCollection<IExecution> _executions;
        private readonly ConcurrentQueue<IExecution> _executionQueue;
        private readonly PeriodicTimer _timer;
        private readonly CancellationTokenSource _cts;

        public Scheduler()
        {
            _executions = new();
            _executionQueue = new();
            _timer = new(TimeSpan.FromMilliseconds(100));
            _cts = new();
        }

        [Initializable(InitializationPriority.Config)]
        public void Initialize()
        {
            var task = StartAsync();

            if (!task.IsCompletedSuccessfully)
                task.FireAndForget();
        }

        public async Task StartAsync()
        {
            while (await _timer.WaitForNextTickAsync(_cts.Token).ConfigureAwait(false))
                _ = ProcessPoolAsync(DateTime.Now).ConfigureAwait(false);
        }

        private Task ProcessPoolAsync(DateTime now)
        {
            var activeTasks = _executions.Where(scheduledTask => scheduledTask.IsAlive(now))
                .Select(scheduledTask => scheduledTask.ExecuteAsync(now)).ToList();

            var unactiveExecutions = new List<IExecution>();
            while (_executionQueue.TryDequeue(out var removedTask))
                if (removedTask.IsAlive(now))
                    activeTasks.Add(removedTask.ExecuteAsync(now));
                else
                    unactiveExecutions.Add(removedTask);

            foreach (var exec in unactiveExecutions)
                _executionQueue.Enqueue(exec);

            return Task.WhenAll(activeTasks);
        }

        public Task StopAsync()
        {
            _cts.Cancel();
            _timer.Dispose();
            _executions.Clear();
            _executionQueue.Clear();

            return Task.CompletedTask;
        }

        public IExecution ExecuteDelayed(Delegate method)
        {
            var execution = new Execution(method);
            _executionQueue.Enqueue(execution);

            return execution;
        }

        public IExecution ExecuteDelayed(Func<Task> method)
        {
            var execution = new Execution(method);
            _executionQueue.Enqueue(execution);

            return execution;
        }

        public IExecution ExecutePeriodically(Delegate method)
        {
            var execution = new Execution(method);
            _executions.Add(execution);

            return execution;
        }

        public IExecution ExecutePeriodically(Func<Task> method)
        {
            var execution = new Execution(method);
            _executions.Add(execution);

            return execution;
        }

        public void CancelExecutionDelayed(IExecution execution)
        {
            var executions = new List<IExecution>();

            while (_executionQueue.TryDequeue(out var exe))
                if (exe != execution)
                    executions.Add(exe);

            foreach (var exe in executions)
                _executionQueue.Enqueue(exe);
        }

        public void CancelExecutionPeriodically(IExecution execution) =>
            _executions.Remove(execution);
    }
}
