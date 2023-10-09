namespace Rollback.Common.DesignPattern.Threading.Schedul.Callback
{
    internal sealed class Execution : IExecution
    {
        private readonly List<DayOfWeek> _days;
        private readonly Delegate? _method;
        private readonly Func<Task>? _asyncMethod;

        private Func<bool>? _condition;
        private TimeSpan _interval;
        private DateTime _nextTick;
        private DayOfWeek _lastTickDay;

        public bool IsAsync =>
            _method is null;

        internal Execution(Delegate method) =>
            (_days, _method) = (new(), method);

        internal Execution(Func<Task> asyncMethod) =>
            (_days, _asyncMethod) = (new(), asyncMethod);

        public bool IsAlive(DateTime now) =>
            (_days.Count is 0 || (_days.Contains(DateTime.Now.DayOfWeek) && _lastTickDay != now.DayOfWeek)) && _nextTick <= now;

        public async Task ExecuteAsync(DateTime now)
        {
            _lastTickDay = now.DayOfWeek;
            _nextTick = now + _interval;

            if (IsAsync && _asyncMethod is not null && (_condition is null || _condition()))
                await _asyncMethod();

            if (!IsAsync && _method is not null && (_condition is null || _condition()))
                _method.DynamicInvoke();
        }

        public IExecution WithTime(TimeSpan time)
        {
            _interval = time;
            _nextTick = DateTime.Now + _interval;

            return this;
        }

        public IExecution WithDays(params DayOfWeek[] days)
        {
            _days.AddRange(days);

            return this;
        }

        public IExecution WithCondition(Func<bool> condition)
        {
            _condition = condition;

            return this;
        }
    }
}
