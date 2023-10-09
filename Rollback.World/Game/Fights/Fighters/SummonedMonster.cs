using Rollback.Protocol.Enums;
using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;
using Rollback.World.Game.RolePlayActors.Monsters;
using Rollback.World.Game.World.Maps;

namespace Rollback.World.Game.Fights.Fighters
{
    public class SummonedMonster : SummonedFighter
    {
        private readonly Monster _monster;

        public override short Level =>
            _monster.Level;

        public override FightTeamMemberInformations FightTeamMemberInformations =>
            new FightTeamMemberMonsterInformations(Id, _monster.Id, _monster.GradeId);

        public SummonedMonster(Monster monster, Cell cell, DirectionsEnum direction, FightActor summoner)
            : base(monster.Look.Clone(), monster.Stats, new(monster.Spells), cell, direction, summoner)
        {
            _monster = monster;

            if (_monster.Race is 0)
                AdjustStats(this);
        }

        public override GameFightFighterInformations GameFightFighterInformations(FightActor fighter) =>
            new GameFightMonsterInformations(Id, Look.GetEntityLook(), EntityDispositionInformations(fighter),
                (sbyte)Team!.Side, Alive, GameFightMinimalStats, _monster.Id, _monster.GradeId);

        private static void AdjustStats(SummonedMonster monster)
        {
            var coeff = 1 + monster.Summoner.Level / 100d;

            monster.Stats.Health.BaseMax = (int)Math.Floor(monster.Stats.Health.ActualMax * coeff);
            monster.Stats.Health.Actual = monster.Stats.Health.ActualMax;

            monster.Stats[Stat.Strength].Base = (short)Math.Floor(monster.Stats[Stat.Strength].Base * coeff);
            monster.Stats[Stat.Intelligence].Base = (short)Math.Floor(monster.Stats[Stat.Intelligence].Base * coeff);
            monster.Stats[Stat.Chance].Base = (short)Math.Floor(monster.Stats[Stat.Chance].Base * coeff);
            monster.Stats[Stat.Agility].Base = (short)Math.Floor(monster.Stats[Stat.Agility].Base * coeff);
        }
    }
}
