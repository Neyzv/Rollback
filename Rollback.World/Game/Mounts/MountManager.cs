using System.Collections.Concurrent;
using Rollback.Common.DesignPattern.Instance;
using Rollback.Common.Initialization;
using Rollback.Common.Logging;
using Rollback.Common.ORM;
using Rollback.World.Database.Mounts;
using Rollback.World.Database.World;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.World.Maps;

namespace Rollback.World.Game.Mounts
{
    public sealed class MountManager : Singleton<MountManager>
    {
        private readonly Dictionary<short, MountRecord> _mountRecords = new();
        private readonly ConcurrentDictionary<int, Mount> _mounts = new();
        private readonly ConcurrentQueue<Mount> _mountsToDelete = new();
        private readonly ConcurrentDictionary<int, Paddock> _paddocks = new();

        private UniqueIdProvider _idProvider = new();

        [Initializable(InitializationPriority.DependantDatasManager, "Mounts")]
        public void Initialize()
        {
            Logger.Instance.Log("\tLoading mounts...");
            foreach (var record in DatabaseAccessor.Instance.Select<MountRecord>(MountRelator.GetAll))
                _mountRecords[record.Id] = record;

            var ids = new HashSet<int>();
            foreach (var mount in DatabaseAccessor.Instance.Select<AccountMountRecord>(CharacterMountRelator.GetAllMounts))
            {
                if (GetMountRecordById(mount.ModelId) is { } template)
                {
                    _mounts[mount.Id] = new Mount(template, mount);
                    ids.Add(mount.Id);
                }
            }

            _idProvider = new UniqueIdProvider(ids);

            Logger.Instance.Log("\tLoading paddocks...");
            foreach (var paddockRecord in DatabaseAccessor.Instance.Select<PaddockRecord>(PaddockRelator.GetAllPaddocks))
                _paddocks[paddockRecord.MapId] = new Paddock(paddockRecord,
                    DatabaseAccessor.Instance.Select<PaddockItemRecord>(string.Format(PaddockItemRelator.GetItemsByMapId, paddockRecord.MapId)));
        }

        public Mount CreateMount(Character owner, MountRecord template)
        {
            var mount = new Mount(template, new AccountMountRecord()
            {
                Id = _idProvider.Generate(),
                ModelId = template.Id,
                Name = "",
                AccountId = owner.Client.Account!.Id,
                Sex = Random.Shared.Next(2) == 1
            });

            _mounts.AddOrUpdate(mount.Id, mount, (_, _) => mount);

            return mount;
        }

        public void DeleteMount(int id)
        {
            if (_mounts.TryRemove(id, out var mount))
                _mountsToDelete.Enqueue(mount);
        }

        public MountRecord? GetMountRecordById(short id) =>
            _mountRecords.TryGetValue(id, out var template) ? template : null;

        public MountRecord? GetMountRecord(Predicate<MountRecord> p) =>
            _mountRecords.Values.FirstOrDefault(x => p(x));

        public Mount? GetMountById(int id) =>
            _mounts.TryGetValue(id, out var mount) ? mount : default;

        public Mount[] GetMounts(Predicate<Mount>? p) =>
            (p is null ? _mounts.Values : _mounts.Values.Where(x => p(x))).ToArray();

        public Paddock? GetPaddock(int mapId) =>
            _paddocks.TryGetValue(mapId, out var paddock) ? paddock : null;

        public void Save()
        {
            while(_mountsToDelete.TryDequeue(out var mountToDelete))
                mountToDelete.Delete();
            
            foreach(var mount in _mounts.Values)
                mount.Save();
            
            foreach(var paddock in _paddocks.Values)
                paddock.Save();
        }
    }
}
