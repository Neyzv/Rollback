using System.Reflection;
using Rollback.Common.DesignPattern.Assemblies;
using Rollback.Common.DesignPattern.Attributes;
using Rollback.Common.DesignPattern.Instance;
using Rollback.Common.Initialization;
using Rollback.Common.Logging;
using Rollback.Common.ORM;
using Rollback.World.Database.World;
using Rollback.World.Game.Mounts;
using Rollback.World.Game.World.Areas;
using Rollback.World.Game.World.Maps;
using Rollback.World.Game.World.Maps.Triggers;

namespace Rollback.World.Game.World
{
    public sealed class WorldManager : Singleton<WorldManager>
    {
        public const int InteractiveAnimateDurationTime = 20_000;

        private readonly Dictionary<short, SuperArea> _superAreas = new();
        private readonly Dictionary<short, Area> _areas = new();
        private readonly Dictionary<short, SubArea> _subAreas = new();
        private readonly Dictionary<int, Map> _maps = new();
        private readonly Dictionary<string, Func<WorldCellsTriggersRecord, CellTrigger>> _cellsTriggersFactories = new();

        [Initializable(InitializationPriority.DependantDatasManager, "World")]
        public void Initialize()
        {
            Logger.Instance.Log("\tLoading super areas...");
            foreach (var superAreaRecord in DatabaseAccessor.Instance.Select<SuperAreaRecord>(SuperAreaRelator.GetSuperAreas))
                _superAreas.Add(superAreaRecord.Id, new(superAreaRecord));

            Logger.Instance.Log("\tLoading areas...");
            foreach (var areaRecord in DatabaseAccessor.Instance.Select<AreaRecord>(AreaRelator.GetAreas))
            {
                if (_superAreas.TryGetValue(areaRecord.SuperAreaId, out var area))
                    _areas.Add(areaRecord.Id, new Area(areaRecord, area));
                else
                    Logger.Instance.LogError(msg: $"Can not find super area {areaRecord.SuperAreaId} for area {areaRecord.Id}");
            }

            Logger.Instance.Log("\tLoading sub areas...");
            foreach (var subAreaRecord in DatabaseAccessor.Instance.Select<SubAreaRecord>(SubAreaRelator.GetSubAreas))
            {
                if (_areas.TryGetValue(subAreaRecord.AreaId, out var area))
                    _subAreas.Add(subAreaRecord.Id, new(subAreaRecord, area));
                else
                    Logger.Instance.LogError(msg: $"Can not find area {subAreaRecord.AreaId} for area {subAreaRecord.Id}");
            }

            Logger.Instance.Log("\tLoading maps...");
            foreach (var mapRecord in DatabaseAccessor.Instance.Select<MapRecord>(MapRelator.GetMaps))
                _maps.Add(mapRecord.Id, new Map(mapRecord, _subAreas.TryGetValue(mapRecord.SubAreaId, out var area) ? area : default));

            Logger.Instance.Log("\tLoading triggers...");
            // Loading templates
            var cellTriggerType = typeof(CellTrigger);
            var worldCellsTriggersRecordType = typeof(WorldCellsTriggersRecord);
            foreach (var (type, attributes) in from assembly in AssemblyManager.Instance.Assemblies
                                               from type in assembly.GetTypes()
                                               let attributes = type.GetCustomAttributes<IdentifierAttribute>()
                                               where !type.IsAbstract && type.IsSubclassOf(cellTriggerType) && attributes.Any()
                                               select (type, attributes))
            {
                var ctor = type.GetConstructor(new[] { worldCellsTriggersRecordType });

                if (ctor is null)
                    Logger.Instance.LogError(msg: $"Can not find a valid constructor for type {type.Name}...");
                else
                {
                    CellTrigger cellTriggerFactory(WorldCellsTriggersRecord record) => (CellTrigger)ctor.Invoke(new[] { record });

                    foreach (var attribute in attributes)
                        if (attribute.Identifier is string identifier)
                            if (_cellsTriggersFactories.ContainsKey(identifier))
                                Logger.Instance.LogWarn(msg: $"Found two cell trigger with identifier {identifier}...");
                            else
                                _cellsTriggersFactories[identifier] = cellTriggerFactory;
                }
            }

            //Binding
            foreach (var cellTrigger in DatabaseAccessor.Instance.Select<WorldCellsTriggersRecord>(WorldCellsTriggersRelator.GetQueries))
            {
                var map = GetMapById(cellTrigger.MapId);
                if (map is null)
                    Logger.Instance.LogError(msg: $"Can not find map {cellTrigger.MapId} to spawn cell trigger...");
                else
                {
                    var trigger = CreateCellTrigger(cellTrigger);
                    if (trigger is null)
                        Logger.Instance.LogError(msg: $"Can not find a trigger with alias {cellTrigger.Action}...");
                    else
                        map.AddCellTrigger(trigger);
                }
            }
        }

        public static short GetDistanceCost(short x1, short y1, short x2, short y2) =>
            (short)Math.Floor(Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2)) * 10);

        public Map? GetMapById(int id) =>
            _maps.ContainsKey(id) ? _maps[id] : default;

        public Map[] GetMaps(Predicate<Map>? p = default) =>
            (p is null ? _maps.Values : _maps.Values.Where(x => p(x))).ToArray();

        public Map? GetMapIdFromCoord(short superAreaId, sbyte x, sbyte y) =>
            _maps.Values.FirstOrDefault(map => map.SubArea?.Area.SuperArea.Id == superAreaId && map.Record.X == x && map.Record.Y == y);

        public CellTrigger? CreateCellTrigger(WorldCellsTriggersRecord record)
        {
            CellTrigger? cellTrigger = null;

            if (_cellsTriggersFactories.TryGetValue(record.Action, out var factory))
                cellTrigger = factory(record);

            return cellTrigger;
        }
    }
}
