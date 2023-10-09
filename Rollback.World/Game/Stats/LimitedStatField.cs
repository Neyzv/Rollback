namespace Rollback.World.Game.Stats
{
    public abstract class LimitedStatField : StatField
    {
        public abstract short Limit { get; }

        public override short TotalWithOutContext =>
            base.TotalWithOutContext > Limit ? Limit : base.TotalWithOutContext;

        protected LimitedStatField(StatsData stats) : base(stats) { }
    }
}
