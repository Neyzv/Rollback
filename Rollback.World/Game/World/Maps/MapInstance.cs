using System.Collections.Concurrent;
using Rollback.Common.DesignPattern.Instance;
using Rollback.Common.Network;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Enums;
using Rollback.Protocol.Types;
using Rollback.World.Database.Dungeons;
using Rollback.World.Database.Interactives;
using Rollback.World.Database.Npcs;
using Rollback.World.Extensions;
using Rollback.World.Game.Criterion;
using Rollback.World.Game.Fights;
using Rollback.World.Game.Fights.Fighters;
using Rollback.World.Game.Fights.Teams;
using Rollback.World.Game.Interactives;
using Rollback.World.Game.Interactives.Skills;
using Rollback.World.Game.Items.Types;
using Rollback.World.Game.Mounts;
using Rollback.World.Game.RolePlayActors;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.RolePlayActors.Monsters;
using Rollback.World.Game.RolePlayActors.Npcs;
using Rollback.World.Game.RolePlayActors.TaxCollectors;
using Rollback.World.Game.World.Maps.Triggers;
using Rollback.World.Game.World.Maps.Triggers.Actions;
using Rollback.World.Handlers.Context;
using Rollback.World.Handlers.Interactives;
using Rollback.World.Handlers.Maps;
using Rollback.World.Handlers.Mounts;
using Rollback.World.Network;

namespace Rollback.World.Game.World.Maps
{
    public class MapInstance : ClientCollection<WorldClient, Message>
    {
        public const byte MaxItemByMap = 40;

        private readonly bool _canBeDeleted;
        private readonly UniqueIdProvider _entityIdProvider;
        private readonly ConcurrentDictionary<int, RolePlayActor> _actors;
        private readonly ConcurrentDictionary<int, InteractiveObject> _interactives;
        private readonly ConcurrentDictionary<short, MapItem> _items;
        private readonly ConcurrentDictionary<short, List<CellTrigger>> _cellsTriggers;
        private readonly ConcurrentDictionary<int, IFight> _fights;
        private readonly ConcurrentDictionary<int, (DungeonRecord, List<DungeonSpawnRecord>)> _dungeons;
        private readonly ConcurrentDictionary<short, MapObstacle> _obstacles;

        private Timer? _spawningTimer;
        private Timer? _dungeonSpawningTimer;

        public Map Map { get; }

        public TaxCollectorNpc? TaxCollector { get; private set; }

        public bool CanBeDeleted =>
            _canBeDeleted && (GetClients().TryGetNonEnumeratedCount(out var count) ? count : GetClients().Count()) is 0;

        public bool CanSpawn =>
            !Map.Record.SpawnDisabled && Map.AllowFights && GetActors<MonsterGroup>(x => !x.DungeonId.HasValue).Length < MonsterConfig.Instance.MaxMonsterGroupsByMap;

        public bool AllowTaxCollector =>
            Map.AllowFights && !ContainsZaap;

        public bool IsDungeon =>
            _dungeons.Count is not 0;

        public bool ContainsZaap { get; private set; }

        public MapInstance(Map map, bool canBeDeleted)
        {
            _canBeDeleted = canBeDeleted;

            Map = map;

            _actors = new();
            _interactives = new();
            _items = new();
            _cellsTriggers = new();
            _fights = new();
            _dungeons = new();
            _obstacles = new();

            _entityIdProvider = new();
        }

        #region Actors
        public T? GetActor<T>(int id)
            where T : RolePlayActor
        {
            var result = default(T?);

            _actors.TryGetValue(id, out var actor);

            if (actor is T castedActor)
                result = castedActor;

            return result;
        }

        public T? GetActor<T>(Predicate<T> p)
            where T : RolePlayActor =>
            (T?)_actors.Values.FirstOrDefault(x => x is T TActor && p(TActor));

        public T[] GetActors<T>(Predicate<T>? p = default) where T : RolePlayActor =>
            _actors.Values.Where(x => x is T tActor && (p is null || p(tActor))).Select(x => (T)x).ToArray();

        protected override IEnumerable<WorldClient> GetClients() =>
            GetActors<Character>().Select(x => x.Client);

        public void AddCharacter(Character character)
        {
            if (_actors.TryAdd(character.Id, character))
            {
                Send(MapHandler.SendGameRolePlayShowActorMessage, new object[] { character });
                ContextHandler.SendCurrentMapMessage(character.Client);

                MapHandler.SendMapFightCountMessage(character.Client, (short)_fights.Count);

                if (ContainsZaap && !character.KnownZaaps.ContainsKey(Map.Record.Id))
                {
                    character.KnownZaaps[Map.Record.Id] = Map;
                    //Tu viens de mémoriser un nouveau zaap.
                    character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 24);
                }

                EnableSpawningPool();

                CheckAutoMove();
            }
        }

        public Npc AddNpc(NpcRecord record, short cellId, DirectionsEnum direction, CriterionExpression? criterion = default)
        {
            var npc = new Npc(-_entityIdProvider.Generate(), this, Map.Record.Cells[cellId], direction, record, criterion);

            if (_actors.TryAdd(npc.Id, npc))
                npc.Refresh();

            return npc;
        }

        public MonsterGroup? AddMonsterGroup(List<Monster> monsters, short cellId, DirectionsEnum direction, short ageBonus = 0, int? dungeonId = null)
        {
            var enabled = _spawningTimer is not null || _dungeonSpawningTimer is not null;
            DisableSpawningPool();

            var result = default(MonsterGroup);

            if (monsters.Count is not 0)
            {
                result = new MonsterGroup(-_entityIdProvider.Generate(), this, Map.Record.Cells[cellId], direction, monsters, dungeonId);

                if (ageBonus is > 0 and < WorldObject.StarBonusLimit)
                    result.AgeBonus = ageBonus;

                if (_actors.TryAdd(result.Id, result))
                    result.Refresh();

                CheckAutoMove();
            }

            if (enabled)
                EnableSpawningPool();

            return result;
        }

        public MonsterGroup? AddMonsterGroup(List<Monster> monsters, short ageBonus = 0, int? dungeonId = null) =>
            AddMonsterGroup(monsters, GetRandomFreeCell(true).Id, DirectionsEnum.DIRECTION_SOUTH.GetDiagonalDecomposition().ElementAt(Random.Shared.Next(2)),
                ageBonus, dungeonId);


        public void AddTaxCollector(TaxCollector taxCollector, Cell? cell = default)
        {
            if (TaxCollector is null)
            {
                var taxCollectorNpc = new TaxCollectorNpc(-_entityIdProvider.Generate(), this, cell ?? GetRandomFreeCell(true), taxCollector);

                if (_actors.TryAdd(taxCollectorNpc.Id, taxCollectorNpc))
                {
                    TaxCollector = taxCollectorNpc;
                    taxCollectorNpc.Refresh();
                }

                CheckAutoMove();
            }
        }

        public void RemoveTaxCollector()
        {
            if (TaxCollector is not null)
                RemoveActor(TaxCollector);
        }

        public void RemoveActor(RolePlayActor actor)
        {
            if (_actors.TryRemove(actor.Id, out _))
            {
                Send(MapHandler.SendGameContextRemoveElementMessage, new object[] { actor.Id });

                if (actor is ContextualActor contextualActor)
                    _entityIdProvider.Free(actor.Id);

                CheckAutoMove();
            }
        }
        #endregion

        private void CheckAutoMove()
        {
            var enable = GetActors<Character>().Length is not 0;
            foreach (var group in GetActors<AutoMoveActor>())
                if (enable)
                    group.EnableAutoMove();
                else
                    group.DisableAutoMove();
        }

        #region Dungeons
        private void OnDungeonGroupEnterFight(MonsterGroup group, IFight fight)
        {
            group.EnterFight -= OnDungeonGroupEnterFight;
            fight.FightEnded += OnDungeonFightEnded;
        }

        private void OnDungeonFightEnded(IFight fight)
        {
            fight.FightEnded -= OnDungeonFightEnded;

            if (fight.Winners.Count is not 0 && fight.Losers.Count is not 0 &&
                fight.Winners.First().Team is CharacterTeam && fight.Losers.First().Team is MonsterTeam monsterTeam
                && monsterTeam.DungeonId.HasValue && _dungeons.ContainsKey(monsterTeam.DungeonId.Value))
                foreach (var fighter in fight.Winners.OfType<CharacterFighter>())
                    fighter.Character.Teleport(_dungeons[monsterTeam.DungeonId.Value].Item1.TeleportMapId,
                        _dungeons[monsterTeam.DungeonId.Value].Item1.TeleportCellId);
        }

        public void AddDungeon((DungeonRecord, List<DungeonSpawnRecord>) dungeonInfos) =>
            _dungeons.TryAdd(dungeonInfos.Item1.Id, dungeonInfos);
        #endregion

        #region SpawningPool
        private void EnableSpawningPool()
        {
            if (CanSpawn)
            {
                if (_dungeonSpawningTimer is null && _dungeons.Count is not 0)
                    _dungeonSpawningTimer = new(new(AutoSpawnDungeon), default, Random.Shared.Next(MonsterConfig.Instance.MinDungeonSpawnInterval,
                        MonsterConfig.Instance.MaxDungeonSpawnInterval), Timeout.Infinite);

                _spawningTimer ??= new(new(AutoSpawn), default, Random.Shared.Next(MonsterConfig.Instance.MinSpawnInterval,
                        MonsterConfig.Instance.MaxSpawnInterval), Timeout.Infinite);
            }
        }

        private void DisableSpawningPool()
        {
            _spawningTimer?.Dispose();
            _spawningTimer = default;

            _dungeonSpawningTimer?.Dispose();
            _dungeonSpawningTimer = default;
        }

        public List<MonsterGroupSize> GetAvailableGroupSizes()
        {
            var takenGroupSizes = GetActors<MonsterGroup>().Select(x => x.Size).ToArray();
            var availableGroupSizes = new List<MonsterGroupSize>();

            foreach (var groupSize in Enum.GetValues(typeof(MonsterGroupSize)))
                if (groupSize is MonsterGroupSize monsterGroupSize && !takenGroupSizes.Contains(monsterGroupSize))
                    availableGroupSizes.Add(monsterGroupSize);

            return availableGroupSizes;
        }

        private void AutoSpawn(object? state)
        {
            if (CanSpawn)
            {
                var availableGroupSizes = GetAvailableGroupSizes();

                MonsterGroupSize size;
                if (availableGroupSizes.Count is not 0)
                    size = availableGroupSizes[Random.Shared.Next(availableGroupSizes.Count)];
                else
                {
                    var groupSizes = Enum.GetValues<MonsterGroupSize>();
                    size = groupSizes[Random.Shared.Next(groupSizes.Length)];
                }

                AddMonsterGroup(MonsterManager.Instance.CreateGroup(Map.Record, size, Map.Record.DefendersCells.Length));
            }
        }

        private void AutoSpawnDungeon(object? state)
        {
            var dungeonsSpawned = GetActors<MonsterGroup>(x => x.DungeonId.HasValue).ToDictionary(x => x.DungeonId!.Value);

            foreach (var dungeon in _dungeons)
            {
                if (!dungeonsSpawned.ContainsKey(dungeon.Key) || dungeonsSpawned[dungeon.Key].IsVisible == false)
                {
                    if (dungeonsSpawned.ContainsKey(dungeon.Key))
                        RemoveActor(dungeonsSpawned[dungeon.Key]);

                    var monsters = new List<Monster>();

                    foreach (var spawnRecord in dungeon.Value.Item2)
                        if (MonsterManager.Instance.GetMonsterRecordById(spawnRecord.MonsterId) is { } record)
                            monsters.Add(new(record, spawnRecord.Grade ?? (sbyte)(Random.Shared.Next(record.Grades.Count) + 1)));

                    var djGroup = AddMonsterGroup(monsters, dungeonsSpawned.ContainsKey(dungeon.Key) ? dungeonsSpawned[dungeon.Key].Cell.Id
                        : (dungeon.Value.Item1.SpawnCellId.HasValue && Cell.CellIdValid(dungeon.Value.Item1.SpawnCellId.Value)
                            ? dungeon.Value.Item1.SpawnCellId.Value : GetFirstFreeCellNearMiddle(true).Id),
                        DirectionsEnum.DIRECTION_SOUTH_EAST, dungeonId: dungeon.Key);

                    if (djGroup is not null)
                        djGroup.EnterFight += OnDungeonGroupEnterFight;
                }
            }
        }
        #endregion

        #region Interactives
        private void ActualizeContainsZaap() =>
            ContainsZaap = _interactives.Any(x => x.Value.Skills.Values.Any(x => x is ZaapSkill));

        public InteractiveObject? GetInteractiveById(int id) =>
            _interactives.ContainsKey(id) ? _interactives[id] : default;

        public InteractiveObject? GetInteractive(Predicate<InteractiveObject> p) =>
            _interactives.Values.FirstOrDefault(x => p(x));

        public InteractiveObject[] GetInteractives(Predicate<InteractiveObject>? p = default) =>
            (p is null ? _interactives.Values : _interactives.Values.Where(x => p(x))).ToArray();

        public void AddInteractive(InteractiveSpawnRecord spawnRecord)
        {
            if (_interactives.TryAdd(spawnRecord.Id, new InteractiveObject(this, spawnRecord)))
            {
                if (!ContainsZaap)
                    ActualizeContainsZaap();

                // if(_interactives[spawnRecord.Id].Skills.OfType<Animate>) To do refresh obstacles

                Send(InteractiveHandler.SendInteractiveMapUpdateMessage, new object[] { this });
            }
        }
        #endregion

        #region Items
        public MapItem[] GetItems(Predicate<MapItem>? p = default) =>
            (p is null ? _items.Values : _items.Values.Where(x => p(x))).ToArray();

        public MapItem? GetMapItemByCellId(short cellId) =>
            _items.ContainsKey(cellId) ? _items[cellId] : default;

        public MapItem? GetItem(Predicate<MapItem> p) =>
            _items.Values.FirstOrDefault(x => p(x));

        public void AddItem(PlayerItem item, short cellId)
        {
            if (!IsItemOnCell(cellId))
            {
                var mapItem = new MapItem(item, Map.Record.Cells[cellId]);

                if (_items.TryAdd(Map.Record.Cells[cellId].Id, mapItem))
                    Send(MapHandler.SendObjectGroundAddedMessage, new object[] { mapItem });
            }
        }

        public void RemoveItem(short cellId)
        {
            if (_items.TryRemove(cellId, out _))
                Send(MapHandler.SendObjectGroundRemovedMessage, new object[] { cellId });
        }
        #endregion

        #region Cells
        public bool IsCellFree(short cellId) =>
            Cell.CellIdValid(cellId) && Map.Record.Cells[cellId].Walkable && !Map.Record.Cells[cellId].NonWalkableDuringFight && (_actors.Count is 0 || _actors.All(x => x.Value.Cell?.Id != cellId));

        public bool IsItemOnCell(short cellId) =>
            _items.Count is not 0 && _items.Any(x => x.Value.Cell.Id == cellId);

        private IEnumerable<Cell> GetFreeCells(bool ignoreActor) =>
            Map.Record.Cells.Where(x => ignoreActor ? x.Walkable && !x.NonWalkableDuringFight : IsCellFree(x.Id));

        public MapObstacle? GetMapObstacleByCellId(short id) =>
            _obstacles.ContainsKey(id) ? _obstacles[id] : default;

        public MapObstacle[] GetObstacles(Predicate<MapObstacle>? p = default) =>
            (p is null ? _obstacles.Values : _obstacles.Values.Where(x => p(x))).ToArray();

        public Cell GetRandomFreeCell(bool ignoreActor = false)
        {
            var cells = GetFreeCells(ignoreActor).ToArray();

            return cells.ElementAt(Random.Shared.Next(cells.Length));
        }

        public Cell? GetRandomAdjacentFreeCell(MapPoint point, bool ignoreActor = false)
        {
            var testedCellIds = new HashSet<int>() { point.CellId };
            var points = new List<MapPoint>(point.GetAdjacentCells());

            while (points.Count is not 0)
            {
                var p = points[0];
                points.RemoveAt(0);

                if (ignoreActor && Map.Record.Cells[p.CellId].Walkable || IsCellFree(p.CellId))
                    return Map.Record.Cells[p.CellId];

                testedCellIds.Add(p.CellId);

                foreach (var adj in p.GetAdjacentCells())
                    if (!testedCellIds.Contains(adj.CellId))
                        points.Add(adj);
            }

            return default;
        }

        public Cell GetFirstFreeCellNearMiddle(bool ignoreActor = false) =>
            GetFreeCells(ignoreActor).OrderBy(x => x.Point.ManhattanDistanceTo(MapPoint.CenterPoint)).First();
        #endregion

        #region CellTriggers
        public TCellTrigger? GetCellTrigger<TCellTrigger>(Predicate<TCellTrigger> p)
            where TCellTrigger : CellTrigger =>
            (TCellTrigger?)_cellsTriggers.SelectMany(x => x.Value).FirstOrDefault(x => x is TCellTrigger cellTrigger && p(cellTrigger));

        public void AddCellTrigger(CellTrigger cellTrigger)
        {
            if (cellTrigger is AnimateTriggerAction animateTrigger)
                foreach (var obstacleId in animateTrigger.ObstacleCellIds)
                    if (!_obstacles.ContainsKey(obstacleId))
                        _obstacles[obstacleId] = new(obstacleId, (sbyte)MapObstacleStateEnum.OBSTACLE_CLOSED);

            if (_cellsTriggers.ContainsKey(cellTrigger.CellId))
                _cellsTriggers[cellTrigger.CellId].Add(cellTrigger);
            else
                _cellsTriggers.TryAdd(cellTrigger.CellId, new() { cellTrigger });
        }

        public void TriggerCell(Character character, short cellId, CellTriggerType type)
        {
            if (_cellsTriggers.ContainsKey(cellId))
                foreach (var trigger in _cellsTriggers[cellId]
                        .OrderByDescending(x => x.Priority)
                        .Where(x => x.Type == type && x.CanExecute(character)))
                    trigger.Trigger(character);
        }
        #endregion

        #region Fights
        public IFight? GetFight(int id) =>
            _fights.ContainsKey(id) ? _fights[id] : default;

        public IFight[] GetFights(Predicate<IFight>? p = default) =>
            (p is null ? _fights.Values : _fights.Values.Where(x => p(x))).ToArray();

        public void AddFight(IFight fight)
        {
            if (_fights.TryAdd(fight.Id, fight))
                Send(MapHandler.SendMapFightCountMessage, new object[] { (short)_fights.Count });
        }

        public void RemoveFight(IFight fight)
        {
            if (_fights.TryRemove(fight.Id, out _))
                Send(MapHandler.SendMapFightCountMessage, new object[] { (short)_fights.Count });
        }
        #endregion
    }
}
