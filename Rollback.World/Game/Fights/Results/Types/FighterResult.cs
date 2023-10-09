using Rollback.Protocol.Enums;
using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;

namespace Rollback.World.Game.Fights.Results.Types
{
    public sealed class FighterResult : FightResult<FightActor>
    {
        public override bool CanDrop =>
            false;

        public override short Wisdom =>
            Looter.Stats[Stat.Wisdom].Total;

        public override short Prospecting =>
            Looter.Stats[Stat.Prospecting].Total;

        public FighterResult(FightActor fighter) : base(fighter)
        {
        }

        public override FightResultFighterListEntry GetResult(FightOutcomeEnum outcome) =>
            new((short)outcome, Loot, Looter.Id, Looter.Alive);
    }
}
