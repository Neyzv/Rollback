using Rollback.Common.DesignPattern.Collections;
using Rollback.Common.DesignPattern.Instance;
using Rollback.Common.Logging;
using Rollback.Common.Network;
using Rollback.Common.Network.Protocol;
using Rollback.World.Database.Dungeons;
using Rollback.World.Database.Interactives;
using Rollback.World.Database.Npcs;
using Rollback.World.Database.World;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.RolePlayActors.TaxCollectors;
using Rollback.World.Game.World.Areas;
using Rollback.World.Game.World.Maps.Triggers;
using Rollback.World.Network;

namespace Rollback.World.Game.World.Maps
{
    public sealed class Map : ClientCollection<WorldClient, Message>
    {
        private readonly SynchronizedCollection<MapInstance> _instances;
        private readonly List<NpcSpawnRecord> _npcsSpawns;
        private readonly Dictionary<int, InteractiveSpawnRecord> _interactivesSpawns;
        private readonly List<CellTrigger> _cellsTriggers;
        private readonly Dictionary<int, (DungeonRecord, List<DungeonSpawnRecord>)> _dungeons;
        private TaxCollector? _taxCollector;

        public MapRecord Record { get; }

        public SubArea? SubArea { get; }

        public Cell[] ChallengersCells { get; }

        public Cell[] DefendersCells { get; }

        public UniqueIdProvider FightIdProvider { get; }

        public bool AllowFights =>
            DefendersCells.Length is not 0 && ChallengersCells.Length is not 0;

        public bool CanSpawnTaxCollector =>
            _taxCollector is null;

        public Map(MapRecord record, SubArea? subArea)
        {
            _instances = new();
            _npcsSpawns = new();
            _interactivesSpawns = new();
            _cellsTriggers = new();
            _dungeons = new();

            Record = record;
            SubArea = subArea;
            ChallengersCells = Record.ChallengersCells.Where(x => Cell.CellIdValid(x) && Record.Cells[x].Walkable).Select(x => Record.Cells[x]).ToArray();
            DefendersCells = Record.DefendersCells.Where(x => Cell.CellIdValid(x) && Record.Cells[x].Walkable).Select(x => Record.Cells[x]).ToArray();
            FightIdProvider = new();

            AddInstance(false);
        }

        protected override IEnumerable<WorldClient> GetClients() =>
            _instances.SelectMany(x => x.GetActors<Character>().Select(y => y.Client));

        public void AddInstance(bool canDelete = true)
        {
            var instance = new MapInstance(this, canDelete);

            foreach (var npcSpawn in _npcsSpawns)
                instance.AddNpc(npcSpawn.Npc!, npcSpawn.CellId, npcSpawn.Direction, npcSpawn.Criterion);

            foreach (var interactiveSpawn in _interactivesSpawns.Values)
                instance.AddInteractive(interactiveSpawn);

            foreach (var cellTrigger in _cellsTriggers)
                instance.AddCellTrigger(cellTrigger);

            foreach (var dungeonInfos in _dungeons.Values)
                instance.AddDungeon(dungeonInfos);

            if (_taxCollector is not null)
                instance.AddTaxCollector(_taxCollector);

            _instances.Add(instance);
        }

        public MapInstance GetMainInstance() =>
            _instances.First();

        public MapInstance GetBestInstance() => // TO DO
            _instances.First();

        public MapInstance[] GetInstances(Predicate<MapInstance>? p = default) =>
            (p is null ? _instances : _instances.Where(x => p(x))).ToArray();

        public void AddNpc(NpcSpawnRecord spawnRecord)
        {
            _npcsSpawns.Add(spawnRecord);

            foreach (var instance in _instances)
                instance.AddNpc(spawnRecord.Npc!, spawnRecord.CellId, spawnRecord.Direction, spawnRecord.Criterion);
        }

        public void AddInteractive(InteractiveSpawnRecord spawnRecord)
        {
            if (_interactivesSpawns.TryAdd(spawnRecord.Id, spawnRecord))
                foreach (var instance in _instances)
                    instance.AddInteractive(spawnRecord);
            else
                Logger.Instance.LogWarn($"Duplicate interactive id {spawnRecord.ElementId}, for map id {Record.Id}...");
        }

        public void AddCellTrigger(CellTrigger cellTrigger)
        {
            _cellsTriggers.Add(cellTrigger);

            foreach (var instance in _instances)
                instance.AddCellTrigger(cellTrigger);
        }

        public void AddDungeon((DungeonRecord, List<DungeonSpawnRecord>) dungeonInfos)
        {
            if (!_dungeons.ContainsKey(dungeonInfos.Item1.Id))
            {
                _dungeons[dungeonInfos.Item1.Id] = dungeonInfos;

                foreach (var instance in _instances)
                    instance.AddDungeon(dungeonInfos);
            }
        }

        public void AddTaxCollector(TaxCollector taxCollector, Cell? cell)
        {
            if (_taxCollector is null)
            {
                _taxCollector = taxCollector;

                foreach (var instance in _instances)
                    instance.AddTaxCollector(taxCollector, cell);
            }
        }

        public void RemoveTaxCollector()
        {
            if (_taxCollector is not null)
            {
                foreach (var instance in _instances)
                    instance.RemoveTaxCollector();

                _taxCollector = null;
            }
        }
    }
}
