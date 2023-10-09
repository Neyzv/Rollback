using System.Reflection;
using Rollback.Common.DesignPattern.Assemblies;
using Rollback.Common.DesignPattern.Instance;
using Rollback.Common.Initialization;
using Rollback.Common.Logging;
using Rollback.Common.ORM;
using Rollback.Protocol.Enums;
using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Characters;
using Rollback.World.Database.Items;
using Rollback.World.Database.Pets;
using Rollback.World.Game.Effects;
using Rollback.World.Game.Items.Attributes;
using Rollback.World.Game.Items.Storages;
using Rollback.World.Game.Items.Types;

namespace Rollback.World.Game.Items
{
    public sealed class ItemManager : Singleton<ItemManager>
    {
        public const byte DefaultItemPosition = (byte)CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED;

        private readonly Dictionary<short, ItemRecord> _records = new();
        private readonly Dictionary<short, ItemSetRecord> _sets = new();
        private readonly Dictionary<short, PetRecord> _pets = new();
        private readonly Dictionary<short, Func<CharacterItemRecord, Inventory, PlayerItem>> _customItemsByType = new();
        private readonly Dictionary<short, Func<CharacterItemRecord, Inventory, PlayerItem>> _customItemsById = new();
        private readonly Dictionary<short, ItemBreedingRecord> _breedingItemsInformations = new();

        public UniqueIdProvider IdProvider { get; private set; } = new();

        [Initializable(InitializationPriority.DependantDatasManager, "Items")]
        public void Initialize()
        {
            Logger.Instance.Log("\tLoading set records...");
            foreach (var set in DatabaseAccessor.Instance.Select<ItemSetRecord>(ItemSetRelator.GetSets))
                _sets[set.Id] = set;

            Logger.Instance.Log("\tLoading records...");
            foreach (var record in DatabaseAccessor.Instance.Select<ItemRecord>(ItemRelator.GetItems))
                _records[record.Id] = record;

            Logger.Instance.Log("\tLoading pets informations...");
            foreach (var petRecord in DatabaseAccessor.Instance.Select<PetRecord>(PetRelator.GetRecords))
            {
                foreach (var foodInformation in DatabaseAccessor.Instance.Select<PetFoodRecord>(
                        string.Format(PetFoodRelator.GetFoodInformationsByPetId, petRecord.PetId)
                    ))
                    if (petRecord.FoodInformations.ContainsKey(foodInformation.Effect.Id))
                        Logger.Instance.LogError(msg: $"Can not assign food information for pet {petRecord.PetId} for effect {foodInformation.Effect.Id}, already one present...");
                    else
                        petRecord.FoodInformations[foodInformation.Effect.Id] = foodInformation;

                _pets[petRecord.PetId] = petRecord;
            }
            
            Logger.Instance.Log("\tLoading breeding items informations...");
            foreach(var breedingItemInfos in DatabaseAccessor.Instance.Select<ItemBreedingRecord>(ItemBreedingRelator.GetAllDatas))
                _breedingItemsInformations.Add(breedingItemInfos.ItemId, breedingItemInfos);

            var itemBaseRecordSubTypes = new List<Type>();

            Logger.Instance.Log("\tLoading custom items...");
            var playerItemType = typeof(PlayerItem);
            var itemBaseRecordType = typeof(ItemBaseRecord);
            foreach (var type in from assembly in AssemblyManager.Instance.Assemblies
                                 from type in assembly.GetTypes()
                                 where !type.IsAbstract
                                 select type)
            {
                if (type.IsSubclassOf(playerItemType))
                {
                    var typeAttr = type.GetCustomAttributes<ItemTypeAttribute>().ToArray();
                    var idAttr = type.GetCustomAttributes<ItemIdAttribute>().ToArray();

                    if (typeAttr.Length is not 0 || idAttr.Length is not 0)
                    {
                        var ctor = type.GetConstructor(new[] { typeof(CharacterItemRecord), typeof(Inventory) });
                        if (ctor is not null)
                        {
                            var itemFactory = (CharacterItemRecord record, Inventory inventory) =>
                            (PlayerItem)ctor.Invoke(new object[] { record, inventory });

                            foreach (var attribute in typeAttr)
                                _customItemsByType[attribute.Id] = itemFactory;

                            foreach (var attribute in idAttr)
                                _customItemsById[attribute.Id] = itemFactory;
                        }
                        else
                            Logger.Instance.LogError(default, $"Can not find a valid constructor for {type.Name}...");
                    }
                }
                else if (type.IsSubclassOf(itemBaseRecordType))
                    itemBaseRecordSubTypes.Add(type);
            }

            Logger.Instance.Log("\tSetting UID Provider...");
            var uids = new HashSet<int>();
            foreach (var type in itemBaseRecordSubTypes)
            {
                var attribute = type.GetCustomAttribute<TableAttribute>();
                if (attribute is not null)
                {
                    var results = DatabaseAccessor.Instance.Select(string.Format(ItemBaseRelator.GetUIDByType, attribute.Name));

                    if (results is not null)
                    {
                        foreach (var result in results)
                        {
                            if (result.ContainsKey("UID"))
                            {
                                var uid = (int)result["UID"]!;

                                if (uids.Contains(uid))
                                    Logger.Instance.LogError(msg: $"Two items with id {uid} where found...");
                                else
                                    uids.Add(uid);
                            }
                            else
                                Logger.Instance.LogError(msg: "Can not find column UID...");
                        }
                    }
                }
                else
                    Logger.Instance.LogError(msg: $"Can not find Table attribute on record, for type {type.Name}...");
            }

            if (uids.Count is not 0)
                IdProvider = new UniqueIdProvider(uids);
        }

        public Dictionary<short, int> ParseItemsInfos(string input, out int totalPrice)
        {
            var items = new Dictionary<short, int>();

            var error = string.Empty;
            totalPrice = 0;

            foreach (var item in input.Split('|'))
            {
                var itemInfo = item.Split(',');
                var quantity = 1;
                var price = 0;

                if (!short.TryParse(itemInfo[0], out var itemId) || itemInfo.Length > 1 && !int.TryParse(itemInfo[1], out quantity) || itemInfo.Length > 2 &&
                    !int.TryParse(itemInfo[2], out price))
                    error = $"Can not parse itemInfos...";
                else
                {
                    var template = GetTemplateRecordById(itemId);

                    if (template is not null)
                    {
                        items[itemId] = quantity;

                        if (itemInfo.Length < 3 || price < 0)
                            price = template.Price;

                        totalPrice += price;
                    }
                    else
                        error = $"Can not find an item with Id {itemId}...";

                    if (!string.IsNullOrWhiteSpace(error))
                        Logger.Instance.LogError(msg: error);
                }
            }

            return items;
        }

        public PlayerItem CreatePlayerItem(CharacterItemRecord record, Inventory inventory)
        {
            if (!_customItemsById.TryGetValue(record.ItemId, out var itemFactory))
                _customItemsByType.TryGetValue((short)record.Template!.TypeId, out itemFactory);

            return itemFactory is null ? new(record, inventory) : itemFactory(record, inventory);
        }

        public PlayerItem CreatePlayerItem(Inventory inventory, ItemRecord template, int quantity, EffectGenerationType generationType,
            IEnumerable<EffectBase>? effects = null)
        {
            var result = CreatePlayerItem(new()
            {
                UID = IdProvider.Generate(),
                ItemId = template.Id,
                Effects = EffectManager.SerializeEffects(effects is null ?
                        template.Effects.Select(x => x.GenerateEffect(generationType, EffectGenerationContext.Item))
                        : effects
                    ),
                Quantity = quantity,
                Position = DefaultItemPosition,
                LastInteractionDate = DateTime.Now
            }, inventory);

            result.OnCreated();

            return result;
        }

        public TaxCollectorItem CreateTaxCollectorItem(TaxCollectorBag taxBag, ItemRecord template, int quantity, EffectGenerationType generationType) =>
            new(new()
            {
                UID = IdProvider.Generate(),
                ItemId = template.Id,
                Effects = EffectManager.SerializeEffects(template.Effects.Select(x => x.GenerateEffect(generationType, EffectGenerationContext.Item))),
                Quantity = quantity,
                LastInteractionDate = DateTime.Now
            }, taxBag);

        public ItemRecord? GetTemplateRecordById(short itemId) =>
            _records.ContainsKey(itemId) ? _records[itemId] : default;

        public ItemRecord[] GetTemplateRecords(Predicate<ItemRecord>? p) =>
            (p is null ? _records.Values : _records.Values.Where(x => p(x))).ToArray();

        public ItemSetRecord? GetSetRecordById(short setId) =>
            _sets.ContainsKey(setId) ? _sets[setId] : default;

        public PetRecord? GetPetRecord(short petId) =>
            _pets.ContainsKey(petId) ? _pets[petId] : default;

        public PetRecord[] GetPetsRecords(Predicate<PetRecord>? p = default) =>
            (p is null ? _pets.Values : _pets.Values.Where(x => p(x))).ToArray();

        public short? GetBreedingItemDurability(short itemId) =>
            _breedingItemsInformations.TryGetValue(itemId, out var infos) ? infos.MaxDurability : null;
    }
}
