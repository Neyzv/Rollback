using System.Collections.Concurrent;
using Rollback.Common.ORM;
using Rollback.Protocol.Types;
using Rollback.World.Database.World;
using Rollback.World.Game.Guilds;
using Rollback.World.Game.Items.Types.Custom;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Handlers.Mounts;

namespace Rollback.World.Game.World.Maps
{
    public sealed class Paddock
    {
        private const int BasePaddockPrice = 2_000_000;
        
        private readonly PaddockRecord _record;
        private readonly ConcurrentDictionary<int, ConcurrentDictionary<short, PaddockItemRecord>> _items = new();
        private readonly ConcurrentQueue<PaddockItemRecord> _itemsToDelete = new();

        public int MapId =>
            _record.MapId;
        
        public bool IsPublic =>
            _record.GuildId < 0;

        public bool IsInSell =>
            _record.Price > 0;

        private Guild? _guild;

        public Guild? Guild =>
            _guild ??= GuildManager.Instance.GetGuildById(_record.GuildId);

        public Paddock(PaddockRecord record, IEnumerable<PaddockItemRecord> items)
        {
            _record = record;
            
            foreach(var item in items)
                AddItem(item);
        }

        public PaddockInformations GetPaddockInformations(Character character) =>
            IsPublic ?
                IsInSell ?
                    new PaddockBuyableInformations(_record.MaxOutdoorMount, _record.MaxItems, _record.Price > 0 ? _record.Price : BasePaddockPrice)
                : new PaddockInformations(_record.MaxOutdoorMount, _record.MaxItems)
            : Guild is null ?
                    new PaddockAbandonnedInformations(_record.MaxOutdoorMount, _record.MaxItems, BasePaddockPrice, _record.GuildId)
            : new PaddockPrivateInformations(_record.MaxOutdoorMount, _record.MaxItems, _record.Price > 0 ? _record.Price : BasePaddockPrice,
                        Guild.Id, Guild.Name, Guild.Emblem);

        public PaddockItem[] GetPaddockItems(Character character) =>
            IsPublic ? 
                _items.TryGetValue(character.Client.Account!.Id, out var items) ? 
                    items.Values.Select(x => x.PaddockItem).ToArray()
                    : Array.Empty<PaddockItem>()
                : _items.Values.SelectMany(x => x.Values.Select(y => y.PaddockItem)).ToArray();

        private bool AddItem(PaddockItemRecord paddockItem)
        {
            var result = false;
            
            if (_items.TryGetValue(paddockItem.OwnerId, out var items))
            {
                if (items.Count < _record.MaxItems && !items.ContainsKey(paddockItem.CellId))
                {
                    items.TryAdd(paddockItem.CellId, paddockItem);
                    
                    result = true;
                }
            }
            else if(_record.MaxItems > 0)
            {
                _items.TryAdd(paddockItem.OwnerId, new ConcurrentDictionary<short, PaddockItemRecord>()
                {
                    [paddockItem.CellId] = paddockItem
                });
                
                result = true;
            }

            return result;
        }

        public bool AddItem(Character owner, BreedingItem item, Cell cell)
        {
            var result = false;
            
            if (item.DurabilityEffect is not null && cell.Point.IsInside(_record.ReferenceCellIds.Select(MapPoint.FromCellId).ToList()))
            {
                var paddockItem = new PaddockItemRecord()
                {
                    OwnerId = owner.Client.Account!.Id,
                    ItemId = item.Id,
                    CellId = cell.Id,
                    Durability = item.DurabilityEffect.Value
                };
                
                result = AddItem(paddockItem);
                
                if (result)
                    if (IsPublic)
                        MountHandler.SendGameDataPaddockObjectAddMessage(owner.Client, paddockItem);
                    else
                        owner.MapInstance.Map.Send(MountHandler.SendGameDataPaddockObjectAddMessage, new object[] { paddockItem });
            }

            return result;
        }

        public void Save()
        {
            while (_itemsToDelete.TryDequeue(out var item))
                DatabaseAccessor.Instance.Delete(item);
            
            foreach (var item in _items.Values.SelectMany(x => x.Values.Select(y => y)))
                DatabaseAccessor.Instance.InsertOrUpdate(item);

            DatabaseAccessor.Instance.InsertOrUpdate(_record);
        }
    }
}
