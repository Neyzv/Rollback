using Rollback.Protocol.Enums;
using Rollback.World.Game.RolePlayActors.Monsters;
using Rollback.World.Game.World.Maps;

namespace Rollback.World.Game.Fights.Fighters
{
    public sealed class SummonedStaticMonster : SummonedMonster
    {
        public override bool CanPlay =>
            false;

        public SummonedStaticMonster(Monster monster, Cell cell, DirectionsEnum direction, FightActor summoner) : base(monster, cell, direction, summoner) { }

        protected override void OnSummonJoinFight() { }
    }
}
