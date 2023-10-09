using Rollback.World.Game.Fights.Fighters;
using Rollback.World.Game.Fights.Teams;
using Rollback.World.Game.Fights.Types;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.RolePlayActors.Monsters;
using Rollback.World.Game.RolePlayActors.TaxCollectors;
using Rollback.World.Game.World.Maps;

namespace Rollback.World.Game.Fights
{
    public static class FightManager
    {
        public const byte MaxFightersByTeam = 8;

        public static FightDuel? CreateDuel(MapInstance map, Character challenger, Character defender, bool activateBlades = true)
        {
            var fight = default(FightDuel);
            if (map.Map.AllowFights && challenger.Fighter is null)
            {
                var challengers = new CharacterTeam(map.Map.ChallengersCells);
                var defenders = new CharacterTeam(map.Map.DefendersCells);
                fight = new FightDuel((short)map.Map.FightIdProvider.Generate(), map, challengers, defenders, activateBlades);

                challenger.JoinTeam(challengers);
                defender.JoinTeam(defenders);
            }

            return fight;
        }

        public static FightPvM? CreatePvM(MapInstance map, Character challenger, IEnumerable<Monster> defender, bool activateBlades = true)
        {
            var fight = default(FightPvM);
            if (map.Map.AllowFights && challenger.Fighter is null)
            {
                var challengers = new CharacterTeam(map.Map.ChallengersCells);
                var defenders = new MonsterTeam(map.Map.DefendersCells);
                fight = new FightPvM((short)map.Map.FightIdProvider.Generate(), map, challengers, defenders, activateBlades);

                foreach (var monster in defender)
                    defenders.AddFighter(new MonsterFighter(fight.GetFreeContextualId(), monster, challenger.Cell));

                challenger.JoinTeam(challengers);
            }

            return fight;
        }

        public static FightPvM? CreatePvM(MapInstance map, Character challenger, MonsterGroup defender, bool activateBlades = true)
        {
            defender.ChangeVisibility(false);

            var fight = CreatePvM(map, challenger, defender.Monsters, activateBlades);

            if (fight is not null && fight.Defenders is MonsterTeam monsterTeam)
            {
                monsterTeam.DungeonId = defender.DungeonId;
                monsterTeam.GroupId = defender.Id;
                defender.OnEnterFight(fight);
            }
            else
                defender.ChangeVisibility(true);

            return fight;
        }

        public static FightPvT? CreatePvT(MapInstance map, Character challenger, TaxCollector taxCollector, bool activateBlades = true)
        {
            var fight = default(FightPvT);

            if (map.Map.AllowFights && challenger.Fighter is null)
            {
                var challengers = new TaxCollectorAttackerTeam(map.Map.ChallengersCells);
                var defenders = new TaxCollectorDefenderTeam(map.Map.DefendersCells);

                fight = new FightPvT((short)map.Map.FightIdProvider.Generate(), map, challengers, defenders, activateBlades, taxCollector);

                taxCollector.JoinTeam(defenders, challenger.Cell);
                challenger.JoinTeam(challengers);
            }

            return fight;
        }
    }
}
