using Rollback.World.CustomEnums;

namespace Rollback.World.Game.Stats.Fields
{
    public sealed class HealthField
    {
        private readonly StatsData _stats;

        private int _baseMax;
        public int BaseMax
        {
            get => _baseMax;
            set =>
                _baseMax = value < 1 ? 1 : value;
        }

        private int _actual;
        public int Actual
        {
            get => _actual;
            set => _actual = value > ActualMax ? ActualMax : value < 0 ? 0 : value;
        }

        public int ActualMax =>
            BaseMax + _stats[Stat.Vitality].Total;

        public HealthField(StatsData stats, int baseMax, int actual)
        {
            _stats = stats;

            _baseMax = baseMax;
            _actual = actual;
        }
    }
}
