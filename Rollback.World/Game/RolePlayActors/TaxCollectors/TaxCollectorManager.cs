using System.Collections.Concurrent;
using Rollback.Common.DesignPattern.Instance;
using Rollback.Common.Extensions;
using Rollback.Common.Initialization;
using Rollback.Common.Logging;
using Rollback.Common.ORM;
using Rollback.World.Database.TaxCollectors;
using Rollback.World.Game.Guilds;
using Rollback.World.Game.World;

namespace Rollback.World.Game.RolePlayActors.TaxCollectors
{
    public sealed class TaxCollectorManager : Singleton<TaxCollectorManager>
    {
        public const int MaxGatheredXP = 2_000_000;
        public const int MaxGatheredXPPerFight = 150_000;
        public const int MinutesBeforeLeavingExchange = 5;

        public const short BaseAP = 6;
        public const short BaseMP = 5;

        private readonly HashSet<short> _taxCollectorsFirstNames;
        private readonly HashSet<short> _taxCollectorsLastNames;
        private readonly ConcurrentDictionary<int, TaxCollector> _taxCollectors;
        private readonly ConcurrentQueue<TaxCollector> _taxCollectorsToDelete;

        private UniqueIdProvider _idProvider;

        public TaxCollectorManager()
        {
            _taxCollectorsFirstNames = new();
            _taxCollectorsLastNames = new();
            _taxCollectors = new();
            _taxCollectorsToDelete = new();
            _idProvider = new();
        }

        [Initializable(InitializationPriority.LowLevelDatasManager, "TaxCollectors")]
        public void Initialize()
        {
            Logger.Instance.Log("\tLoading names...");
            foreach (var firstName in DatabaseAccessor.Instance.Select("select * from tax_collectors_first_names"))
                if (firstName.TryGetValue("Id", out var firstNameO) && firstNameO.ChangeType<short>() is { } firstNameId)
                    _taxCollectorsFirstNames.Add(firstNameId);

            foreach (var lastName in DatabaseAccessor.Instance.Select("select * from tax_collectors_first_names"))
                if (lastName.TryGetValue("Id", out var lastNameO) && lastNameO.ChangeType<short>() is { } lastNameId)
                    _taxCollectorsLastNames.Add(lastNameId);

            Logger.Instance.Log("\tSpawning TaxCollectors...");
            var ids = new HashSet<int>();

            foreach (var taxCollector in DatabaseAccessor.Instance.Select<TaxCollectorRecord>(TaxCollectorRelator.GetTaxCollectors))
                if (taxCollector.Hirer?.Guild is not null && WorldManager.Instance.GetMapById(taxCollector.MapId) is { } map &&
                    !_taxCollectors.ContainsKey(taxCollector.MapId))
                {
                    var tax = new TaxCollector(taxCollector, map);

                    ids.Add(taxCollector.Id);
                    _taxCollectors.TryAdd(taxCollector.MapId, tax);

                    taxCollector.Hirer.Guild.AddTaxCollector(tax);
                }
                else
                {
                    Logger.Instance.LogInfo($"Deletion of TaxCollector {taxCollector.Id}, incorrect informations...");
                    DatabaseAccessor.Instance.Delete(taxCollector);
                }

            Logger.Instance.Log("\tSetting id provider...");
            if (ids.Count is not 0)
                _idProvider = new UniqueIdProvider(ids);
        }

        public TaxCollector CreateTaxCollector(GuildMember member)
        {
            var taxCollector = new TaxCollector(new()
            {
                Id = _idProvider.Generate(),
                HirerId = member.MemberId,
                FirstNameId = _taxCollectorsFirstNames.ElementAt(Random.Shared.Next(_taxCollectorsFirstNames.Count)),
                LastNameId = _taxCollectorsLastNames.ElementAt(Random.Shared.Next(_taxCollectorsFirstNames.Count)),
                HiredDate = DateTime.Now,
                MapId = member.Character!.MapInstance.Map.Record.Id
            }, member.Character!.MapInstance.Map);

            _taxCollectors.TryAdd(member.Character.MapInstance.Map.Record.Id, taxCollector);

            return taxCollector;
        }

        public void RemoveTaxCollector(TaxCollector taxCollector)
        {
            _taxCollectors.TryRemove(taxCollector.MapId, out _);
            _taxCollectorsToDelete.Enqueue(taxCollector);
        }

        public void Save()
        {
            foreach (var taxCollector in _taxCollectors.Values)
                taxCollector.Save();

            while (_taxCollectorsToDelete.TryDequeue(out var taxCollector))
                taxCollector.Delete();
        }
    }
}
