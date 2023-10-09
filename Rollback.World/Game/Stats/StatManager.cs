using System.Reflection;
using Rollback.Common.DesignPattern.Assemblies;
using Rollback.Common.DesignPattern.Attributes;
using Rollback.Common.DesignPattern.Instance;
using Rollback.Common.Initialization;
using Rollback.Common.Logging;
using Rollback.World.CustomEnums;

namespace Rollback.World.Game.Stats
{
    public sealed class StatManager : Singleton<StatManager>
    {
        private readonly Dictionary<Stat, Func<StatsData, StatField>> _statsFactories;

        public StatManager() =>
            _statsFactories = new();

        [Initializable(InitializationPriority.DatasManager, "Stats")]
        public void Initialize()
        {
            var statFieldType = typeof(StatField);
            var statsDataType = typeof(StatsData);
            foreach (var (type, attributes) in from assembly in AssemblyManager.Instance.Assemblies
                                               from type in assembly.GetTypes()
                                               where !type.IsAbstract && type.IsSubclassOf(statFieldType)
                                               let attributes = type.GetCustomAttributes<IdentifierAttribute>(false)
                                               where attributes.Any()
                                               select (type, attributes))
            {
                var ctor = type.GetConstructor(new[] { statsDataType });
                if (ctor is not null)
                {
                    var statFactory = (StatsData stats) => (StatField)ctor.Invoke(new[] { stats });

                    foreach (var attribute in attributes)
                        if (attribute.Identifier is Stat identifier)
                            if (!_statsFactories.ContainsKey(identifier))
                                _statsFactories[identifier] = statFactory;
                            else
                                Logger.Instance.LogError(msg: $"A special stat already use this attribute...");
                        else
                            Logger.Instance.LogWarn($"Incorrect identifier type in IdentifierAttribute for type {type.Name}...");
                }
                else
                    Logger.Instance.LogError(msg: $"Can not find a valide constructor for type {type.Name}...");
            }
        }

        public StatField CreateStatField(Stat stat, StatsData stats) =>
            _statsFactories.ContainsKey(stat) ? _statsFactories[stat](stats) : new(stats);
    }
}
