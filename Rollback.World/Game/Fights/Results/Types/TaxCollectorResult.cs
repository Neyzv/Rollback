using Rollback.Protocol.Enums;
using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Fights.Fighters;

namespace Rollback.World.Game.Fights.Results.Types
{
    public sealed class TaxCollectorResult : FightResult<TaxCollectorFighter>
    {
        public override bool CanDrop =>
            false;

        public override short Wisdom =>
            Looter.Stats[Stat.Wisdom].Total;

        public override short Prospecting =>
            Looter.Stats[Stat.Prospecting].Total;

        public TaxCollectorResult(TaxCollectorFighter looter) : base(looter) { }

        public override FightResultFighterListEntry GetResult(FightOutcomeEnum outcome) =>
            new FightResultTaxCollectorListEntry((short)outcome, Loot, Looter.Id, Looter.Alive, (byte)Looter.Level,
                Looter.TaxCollector.Guild.Name, 0);
    }
}
