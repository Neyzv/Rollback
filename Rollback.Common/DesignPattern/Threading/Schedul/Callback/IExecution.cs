namespace Rollback.Common.DesignPattern.Threading.Schedul.Callback
{
    public interface IExecution
    {
        bool IsAlive(DateTime now);

        Task ExecuteAsync(DateTime now);

        IExecution WithTime(TimeSpan time);

        IExecution WithDays(params DayOfWeek[] days);

        IExecution WithCondition(Func<bool> condition);
    }
}
