using Rollback.Auth.Database;
using Rollback.Auth.Objects;
using Rollback.Common.DesignPattern.Instance;
using Rollback.Common.Initialization;
using Rollback.Common.ORM;

namespace Rollback.Auth.Managers
{
    public sealed class WorldManager : Singleton<WorldManager>
    {
        private readonly Dictionary<int, World> _worlds = new();

        [Initializable(InitializationPriority.DatasManager, "Worlds")]
        public void Initialize()
        {
            foreach (var record in DatabaseAccessor.Instance.Select<WorldRecord>(WorldRelator.GetQueries))
                _worlds[record.Id] = new(record);
        }

        public World? GetWorldById(int worldId) =>
            _worlds.ContainsKey(worldId) ? _worlds[worldId] : default;

        public World[] GetWorlds(Predicate<World>? p = default) =>
            (p is null ? _worlds.Values : _worlds.Values.Where(x => p(x))).ToArray();
    }
}
