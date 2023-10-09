using Rollback.Common.DesignPattern.Threading.Schedul.Callback;

namespace Rollback.World.Game.ScheduledExecutions.Types.Abstractions
{
    public abstract class BaseScheduledExecution
    {
        public abstract string Identifier { get; }

        public BaseScheduledExecution() { }

        public abstract IExecution Start();
    }
}
