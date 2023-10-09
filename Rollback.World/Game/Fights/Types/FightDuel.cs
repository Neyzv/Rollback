using Rollback.Protocol.Enums;
using Rollback.Protocol.Types;
using Rollback.World.Game.Fights.Teams;
using Rollback.World.Game.World.Maps;

namespace Rollback.World.Game.Fights.Types
{
    public sealed class FightDuel : Fight<CharacterTeam, CharacterTeam>
    {
        public override FightTypeEnum Type =>
            FightTypeEnum.FIGHT_TYPE_CHALLENGE;

        public override bool CanCancelFight =>
            true;

        public FightDuel(short id, MapInstance map, CharacterTeam challengers, CharacterTeam defenders, bool activateBlades)
            : base(id, map, challengers, defenders, activateBlades) { }

        protected override FightResultListEntry[] GenerateResults()
        {
            var result = new FightResultListEntry[Winners.Count + Losers.Count];
            var index = 0;

            foreach (var fighter in Winners)
                result[index++] = fighter.Result.GetResult(FightOutcomeEnum.RESULT_VICTORY);

            foreach (var fighter in Losers)
                result[index++] = fighter.Result.GetResult(FightOutcomeEnum.RESULT_LOST);

            return result;
        }
    }
}
