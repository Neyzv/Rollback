using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;

namespace Rollback.World.Game.Stats.Fields
{
    [Identifier(Stat.AirElementResistPercent),
        Identifier(Stat.EarthElementResistPercent),
        Identifier(Stat.FireElementResistPercent),
        Identifier(Stat.NeutralElementResistPercent),
        Identifier(Stat.WaterElementResistPercent),
        Identifier(Stat.PvpAirElementResistPercent),
        Identifier(Stat.PvpEarthElementResistPercent),
        Identifier(Stat.PvpFireElementResistPercent),
        Identifier(Stat.PvpNeutralElementResistPercent),
        Identifier(Stat.PvpWaterElementResistPercent)]
    public sealed class ResistancePercentField : LimitedStatField
    {
        public override short Limit =>
            50;

        public ResistancePercentField(StatsData stats) : base(stats) { }
    }
}
