using Rollback.Protocol.Enums;
using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Monsters;
using Rollback.World.Game.Fights.Fighters;
using Rollback.World.Game.Fights.Results;
using Rollback.World.Game.Fights.Teams;
using Rollback.World.Game.RolePlayActors.Monsters;
using Rollback.World.Game.World.Maps;

namespace Rollback.World.Game.Fights.Types
{
    public sealed class FightPvM : Fight<CharacterTeam, MonsterTeam>
    {
        public override FightTypeEnum Type =>
            FightTypeEnum.FIGHT_TYPE_PvM;

        public override bool CanCancelFight =>
            false;

        public FightPvM(short id, MapInstance map, CharacterTeam challengers, MonsterTeam defenders, bool activateBlades)
            : base(id, map, challengers, defenders, activateBlades) { }

        private static MonsterDropRecord[] GetPotentialsDrops(MonsterFighter[] monsters, short teamPP) =>
            monsters.SelectMany(x => x.Drops.Where(y => y.LootingFloor <= teamPP)).ToArray();

        protected override FightResultListEntry[] GenerateResults()
        {
            var resultCount = Winners.Count + Losers.Count;

            var looters = Winners.Select(x => x.Result).OrderByDescending(x => x.Prospecting).ToList();
            if (Map.TaxCollector is not null)
            {
                looters.Add(Map.TaxCollector.Result);
                resultCount++;
            }

            var result = new FightResultListEntry[resultCount];
            var index = 0;

            short bonus = AgeBonus; // TO DO Challenges

            var allies = Winners.OfType<CharacterFighter>().ToArray();
            var ennemies = Losers.OfType<MonsterFighter>().ToArray();

            var potentialsDrops = GetPotentialsDrops(ennemies, (short)Math.Floor(allies.Sum(x => x.Stats[Stat.Prospecting].Total) * (1 + bonus / 100d)));
            var alreadyDroppedItems = new Dictionary<short, short>();

            foreach (var loot in looters)
            {
                if (loot is IExperienceFightResult xpResult)
                    xpResult.AddEarnedExperience(FightFormulas.CalculateEarnedExperience(loot, allies, ennemies,
                        bonus, true));

                if (loot.CanDrop)
                {
                    loot.AddEarnedKamas(FightFormulas.CalculateEarnedKamas(ennemies.Sum(x => Random.Shared.Next(x.MinKamas, x.MaxKamas + 1)), bonus));

                    foreach (var drop in FightFormulas.RollDrop(loot, potentialsDrops, ref alreadyDroppedItems))
                        loot.AddEarnedItem(drop.Key, drop.Value);
                }

                loot.Apply();
                result[index++] = loot.GetResult(FightOutcomeEnum.RESULT_VICTORY);
            }

            allies = Losers.OfType<CharacterFighter>().ToArray();
            ennemies = Winners.OfType<MonsterFighter>().ToArray();

            foreach (var fighter in Losers)
            {
                if (fighter.Result is IExperienceFightResult xpResult)
                    xpResult.AddEarnedExperience(FightFormulas.CalculateEarnedExperience(fighter.Result, allies, ennemies, bonus, false));

                result[index++] = fighter.Result.GetResult(FightOutcomeEnum.RESULT_LOST);
                fighter.Result.Apply();
            }

            if (_defenders.GroupId.HasValue && !_defenders.DungeonId.HasValue)
            {
                var group = Map.GetActor<MonsterGroup>(_defenders.GroupId.Value);

                if (group is not null)
                {
                    var challengerId = Challengers.GetFighters<FightActor>().FirstOrDefault()?.Id;

                    if (challengerId is not null && Winners.Any(x => x.Id == challengerId))
                        Map.RemoveActor(group);
                    else
                        group.ChangeVisibility(true);
                }
            }

            return result;
        }
    }
}
