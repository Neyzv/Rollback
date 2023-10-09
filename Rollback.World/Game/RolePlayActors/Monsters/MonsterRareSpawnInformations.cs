using System.Collections.Concurrent;
using Rollback.Common.Logging;
using Rollback.World.Database.Monsters;
using Rollback.World.Game.Fights;
using Rollback.World.Game.Fights.Teams;
using Rollback.World.Game.World;
using Rollback.World.Game.World.Maps;

namespace Rollback.World.Game.RolePlayActors.Monsters
{
    public sealed class MonsterRareSpawnInformations
    {
        private readonly ConcurrentDictionary<int, Map> _maps;
        private readonly MonsterRareSpawnRecord _record;

        private DateTime? _nextSpawn;
        private MonsterGroup? _group;

        public MonsterRareSpawnInformations(MonsterRareSpawnRecord record)
        {
            _record = record;

            _maps = new();
            LoadMaps();
        }

        private void LoadMaps()
        {
            foreach (var mapId in _record.MapIds)
            {
                if (WorldManager.Instance.GetMapById(mapId) is { } map)
                {
                    if (map.AllowFights)
                        _maps.TryAdd(map.Record.Id, map);
                }
                else
                    Logger.Instance.LogError(msg: $"Uknown {nameof(Map)} {mapId} for {nameof(MonsterRareSpawnInformations)} {_record.Id}");
            }

            foreach (var subAreaId in _record.SubAreaIds)
            {
                if (WorldManager.Instance.GetMaps(x => x.SubArea?.Id == subAreaId && x.AllowFights) is { Length: > 0 } maps)
                    foreach (var map in maps)
                        _maps.TryAdd(map.Record.Id, map);
                else
                    Logger.Instance.LogError(msg: $"No available {nameof(Map)}s for {subAreaId}, {nameof(MonsterRareSpawnInformations)} {_record.Id}");
            }
        }

        private void OnFightEnded(IFight fight)
        {
            fight.FightEnded -= OnFightEnded;

            if (fight.Winners.Count is not 0 && fight.Losers.Count is not 0 && fight.Losers.First().Team is MonsterTeam monsterTeam)
            {
                _group = default;
                _nextSpawn = DateTime.Now + TimeSpan.FromMinutes(Random.Shared.Next(_record.MinRespawnMinute, _record.MaxRespawnMinute + 1));
            }
        }

        private void OnRareSpawnEnterFight(MonsterGroup group, IFight fight)
        {
            group.EnterFight -= OnRareSpawnEnterFight;
            fight.FightEnded += OnFightEnded;
        }

        public void TrySpawn()
        {
            if (_group is null && (_nextSpawn is null || _nextSpawn <= DateTime.Now) && _maps.Count is not 0)
            {
                if (MonsterManager.Instance.GetMonsterRecordById(_record.MonsterId) is { } monsterRecord)
                {
                    var monster = new Monster(monsterRecord, (sbyte)Random.Shared.Next(_record.MinGrade, _record.MaxGrade + 1));
                    var map = _maps.Values.ToArray()[Random.Shared.Next(0, _maps.Count)].GetMainInstance();

                    if (map.CanSpawn)
                    {
                        var availableGroupSizes = map.GetAvailableGroupSizes();

                        MonsterGroupSize size;
                        if (availableGroupSizes.Count is not 0)
                            size = availableGroupSizes[Random.Shared.Next(availableGroupSizes.Count)];
                        else
                        {
                            var groupSizes = Enum.GetValues<MonsterGroupSize>();
                            size = groupSizes[Random.Shared.Next(groupSizes.Length)];
                        }

                        var monsters = MonsterManager.Instance.CreateGroup(map.Map.Record, size, map.Map.Record.DefendersCells.Length);
                        if (monsters.Count is not 0)
                            monsters[0] = monster;
                        else
                            monsters.Add(monster);

                        _group = map.AddMonsterGroup(monsters);
                    }
                    else
                    {
                        var monsterGroups = map.GetActors<MonsterGroup>();
                        var group = monsterGroups[Random.Shared.Next(monsterGroups.Length)];
                        var monsters = group.Monsters.ToList();
                        monsters[0] = monster;

                        map.RemoveActor(group);
                        _group = map.AddMonsterGroup(monsters, group.Cell.Id, group.Direction, group.AgeBonus);
                    }

                    if (_group is not null)
                        _group.EnterFight += OnRareSpawnEnterFight;
                }
                else
                    Logger.Instance.LogError(msg: $"Can not find monster {_record.MonsterId} for {nameof(MonsterRareSpawnRecord)} {_record.Id}...");
            }
        }
    }
}
