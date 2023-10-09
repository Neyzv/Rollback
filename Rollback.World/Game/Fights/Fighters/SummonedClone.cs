using Rollback.Protocol.Enums;
using Rollback.Protocol.Types;
using Rollback.World.Game.World.Maps;

namespace Rollback.World.Game.Fights.Fighters
{
    internal class SummonedClone : SummonedFighter
    {
        public override short Level =>
            Summoner.Level;

        public override FightTeamMemberInformations FightTeamMemberInformations =>
            new(Id);

        public SummonedClone(Cell cell, DirectionsEnum direction, FightActor summoner)
            : base(summoner.Look.Clone(), summoner.Stats, new(), cell, direction, summoner)
        {
            Stats = summoner.Stats.Clone();
            Stats.Health.Actual = Stats.Health.ActualMax;
            ResetUsedPoints();
        }

        public override GameFightFighterInformations GameFightFighterInformations(FightActor fighter)
        {
            if (Summoner is CharacterFighter characterFighter)
                return new GameFightCharacterInformations(Id, Look.GetEntityLook(), EntityDispositionInformations(fighter), (sbyte)Team!.Side,
                    Alive, GameFightMinimalStats, characterFighter.Character.Name, Level, characterFighter.Character.ActorAlignmentInformations);

            return new GameFightFighterInformations(Id, Look.GetEntityLook(), EntityDispositionInformations(fighter), (sbyte)Team!.Side,
                    Alive, GameFightMinimalStats);
        }
    }
}
