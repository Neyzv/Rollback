using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;

namespace Rollback.World.Game.Stats.Fields
{
    [Identifier(Stat.ActionPoints),
        Identifier(Stat.MovementPoints)]
    public sealed class UseablePointsField : StatField
    {
        public short Used { get; set; }

        public override short TotalWithOutContext
        {
            get
            {
                var amount = (short)(base.TotalWithOutContext - Used);
                return amount < 0 ? (short)0 : amount;
            }
        }

        public override short Total
        {
            get
            {
                var amount = base.Total;
                return amount < 0 ? (short)0 : amount;
            }
        }

        public short TotalMax
        {
            get
            {
                var amount = (short)(base.TotalWithOutContext + Context);
                return amount < 0 ? (short)0 : amount;
            }
        }

        public UseablePointsField(StatsData stats) : base(stats) { }
    }
}
