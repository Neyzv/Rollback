using Rollback.Common.DesignPattern.Collections;
using Rollback.Common.DesignPattern.Instance;
using Rollback.Common.Initialization;
using Rollback.Common.Logging;
using Rollback.Common.ORM;
using Rollback.World.Database.Dungeons;
using Rollback.World.Database.Monsters;
using Rollback.World.Database.World;
using Rollback.World.Game.Spells;
using Rollback.World.Game.World;

namespace Rollback.World.Game.RolePlayActors.Monsters
{
    public sealed class MonsterManager : Singleton<MonsterManager>
    {
        private readonly Dictionary<short, MonsterRecord> _monsters;
        private readonly Dictionary<short, List<MonsterSpawnRecord>> _spawnsBySubAreaId;
        private readonly Dictionary<int, List<MonsterSpawnRecord>> _spawnsByMapId;
        private readonly SynchronizedCollection<MonsterRareSpawnInformations> _rareSpawns;

        public MonsterManager()
        {
            _monsters = new();
            _spawnsBySubAreaId = new();
            _spawnsByMapId = new();
            _rareSpawns = new();
        }

        [Initializable(InitializationPriority.LowLevelDatasManager, "Monsters")]
        public void Initialize()
        {
            Logger.Instance.Log("\tLoading monsters...");
            foreach (var monster in DatabaseAccessor.Instance.Select<MonsterRecord>(MonsterRelator.GetMonsters))
            {
                monster.Drops = DatabaseAccessor.Instance.Select<MonsterDropRecord>(string.Format(MonsterDropRelator.GetDropsByMonsterId, monster.Id));
                monster.Grades = DatabaseAccessor.Instance.Select<MonsterGradeRecord>(string.Format(MonsterGradeRelator.GetMonsterGradesByMonsterId, monster.Id));

                foreach (var grade in monster.Grades)
                {
                    foreach (var spell in DatabaseAccessor.Instance.Select<MonsterSpellRecord>(string.Format(MonsterSpellRelator.GetSpellByMonsterAndGradeId, grade.MonsterId, grade.Grade)))
                    {
                        var spellTemplate = SpellManager.Instance.GetSpellTemplateById(spell.SpellId);
                        if (spellTemplate is not null && spell.SpellLevel <= spellTemplate.SpellLevels.Length && !grade.Spells.ContainsKey(spell.SpellId))
                            grade.Spells.Add(spell.SpellId, new Spell(spellTemplate, spell.SpellLevel));
                        else
                            Logger.Instance.LogWarn($"Can not assign spell {spell.SpellId} to monster {spell.MonsterId}...");
                    }
                }

                _monsters[monster.Id] = monster;
            }

            Logger.Instance.Log("\tLoading spawns...");
            foreach (var spawn in DatabaseAccessor.Instance.Select<MonsterSpawnRecord>(MonsterSpawnRelator.GetSpawns))
            {
                if (!spawn.Disabled)
                {
                    if (spawn.SubAreaId.HasValue)
                    {
                        if (_spawnsBySubAreaId.ContainsKey(spawn.SubAreaId.Value))
                            _spawnsBySubAreaId[spawn.SubAreaId.Value].Add(spawn);
                        else
                            _spawnsBySubAreaId.Add(spawn.SubAreaId.Value, new() { spawn });
                    }

                    if (spawn.MapId.HasValue)
                    {
                        if (_spawnsByMapId.ContainsKey(spawn.MapId.Value))
                            _spawnsByMapId[spawn.MapId.Value].Add(spawn);
                        else
                            _spawnsByMapId.Add(spawn.MapId.Value, new() { spawn });
                    }
                }
            }

            foreach (var rareSpawn in DatabaseAccessor.Instance.Select<MonsterRareSpawnRecord>(MonsterRareSpawnRelator.QueryAll))
                _rareSpawns.Add(new(rareSpawn));

            Logger.Instance.Log("\tLoading dungeons...");
            foreach (var dungeonRecord in DatabaseAccessor.Instance.Select<DungeonRecord>(DungeonRelator.GetDungeons))
                if (WorldManager.Instance.GetMapById(dungeonRecord.FightMapId) is { } map)
                    map.AddDungeon((dungeonRecord, DatabaseAccessor.Instance.Select<DungeonSpawnRecord>(string.Format(
                        DungeonSpawnRelator.GetDungeonSpawnsByDungeonId, dungeonRecord.Id))));
                else
                    Logger.Instance.LogWarn($"Can not find map {dungeonRecord.FightMapId}, for dungeon {dungeonRecord.Id}...");

            Logger.Instance.Log("\tSpawning monsters...");
            foreach (var map in WorldManager.Instance.GetMaps())
                foreach (var instance in map.GetInstances(x => x.CanSpawn))
                    instance.AddMonsterGroup(CreateRandomGroup(map.Record, map.Record.DefendersCells.Length));
        }

        public void CheckMonstersRareSpawnsAvailability()
        {
            foreach (var spawn in _rareSpawns)
                spawn.TrySpawn();
        }

        private List<Monster> CreateRandomGroup(MapRecord mapRecord, int maxMonstersAmount = 8)
        {
            var values = Enum.GetValues(typeof(MonsterGroupSize));

            return CreateGroup(mapRecord, (MonsterGroupSize)values.GetValue(Random.Shared.Next(values.Length))!, maxMonstersAmount);
        }

        public List<Monster> CreateGroup(MapRecord mapRecord, MonsterGroupSize groupSize, int maxMonstersAmount = 8)
        {
            var monsters = new List<Monster>();
            var nbr = groupSize switch
            {
                MonsterGroupSize.Small => Random.Shared.Next(4),
                MonsterGroupSize.Medium => Random.Shared.Next(4, 6),
                MonsterGroupSize.Big => Random.Shared.Next(6, 8),
                _ => 1,
            };


            if (nbr > maxMonstersAmount)
                nbr = maxMonstersAmount;

            var possibleSpawns = new List<MonsterSpawnRecord>();
            if (_spawnsBySubAreaId.ContainsKey(mapRecord.SubAreaId))
                possibleSpawns.AddRange(_spawnsBySubAreaId[mapRecord.SubAreaId]);

            if (_spawnsByMapId.ContainsKey(mapRecord.Id))
                possibleSpawns.AddRange(_spawnsByMapId[mapRecord.Id]);

            if (possibleSpawns.Count is not 0)
            {
                var maxSpawnPod = possibleSpawns.Max(x => x.Probability) + 1;

                while (monsters.Count < nbr)
                {
                    var spawnPod = Random.Shared.Next(maxSpawnPod);

                    var possibleMonsters = possibleSpawns.Where(x => x.Probability >= spawnPod).ToArray();
                    if (possibleMonsters.Length is not 0)
                    {
                        var selectedMonster = possibleMonsters.ElementAt(Random.Shared.Next(possibleMonsters.Length));

                        var record = GetMonsterRecordById(selectedMonster.MonsterId);
                        if (record is not null)
                            monsters.Add(new(record, (sbyte)Random.Shared.Next(selectedMonster.MinGrade, selectedMonster.MaxGrade + 1)));
                        else
                            Logger.Instance.LogWarn($"Can not add monster {selectedMonster.MonsterId} to a group, record can not be found...");
                    }
                }
            }

            return monsters;
        }

        public static MonsterGroupSize GetGroupSize(int count)
        {
            var result = MonsterGroupSize.Big;

            if (count < 4)
                result = MonsterGroupSize.Small;
            else if (count < 6)
                result = MonsterGroupSize.Medium;

            return result;
        }

        public MonsterRecord? GetMonsterRecordById(short id) =>
            _monsters.ContainsKey(id) ? _monsters[id] : default;
    }
}
