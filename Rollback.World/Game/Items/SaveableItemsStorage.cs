using System.Collections.Concurrent;
using Rollback.Protocol.Enums;
using Rollback.Protocol.Types;
using Rollback.World.Game.Effects;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Items
{
    public abstract class SaveableItemsStorage<TOwner, TRecord, TItem, TStorage>
        where TRecord : ItemBaseRecord
        where TStorage : SaveableItemsStorage<TOwner, TRecord, TItem, TStorage>
        where TItem : SaveableItem<TItem, TRecord, TOwner, TStorage>
        where TOwner : class
    {
        private readonly ConcurrentDictionary<int, KeyValuePair<TItem, bool>> _itemsToDelete;
        protected readonly ConcurrentDictionary<int, TItem> _items;

        protected abstract int OwnerId { get; }

        public TOwner Owner { get; }

        public int Pods =>
            _items.Sum(x => x.Value.Weight * x.Value.Quantity);

        public abstract int MaxPods { get; }

        public bool IsFull =>
            Pods > MaxPods;

        public ObjectItem[] InventoryContent =>
            _items.OrderBy(x => x.Value.LastInteractionDate).Select(x => x.Value.ObjectItem).ToArray();

        public SaveableItemsStorage(TOwner owner)
        {
            _items = new();
            _itemsToDelete = new();

            Owner = owner;

            ItemAdded += OnItemAdded;
            ItemRemoved += OnItemRemovedDelete;

            Load();
        }

        #region Events
        protected event Action<TItem>? ItemAdded;

        protected event Action<TItem, bool>? ItemRemoved;

        protected event Action<TItem>? ItemQuantityChanged;
        #endregion

        private void OnItemAdded(TItem item) =>
            _itemsToDelete.Remove(item.UID, out _);

        private void OnItemRemovedDelete(TItem item, bool delete) =>
            _itemsToDelete.TryAdd(item.UID, new(item, delete));

        protected abstract void Load();

        protected TItem? TryStack(TItem item)
        {
            var result = default(TItem);
            var stack = _items.Values.FirstOrDefault(x => x != item && item.CanStackOn(x));

            if (stack is not null)
            {
                stack.Quantity += item.Quantity;

                ItemRemoved?.Invoke(item, true);
                _items.Remove(item.UID, out _);

                ItemQuantityChanged?.Invoke(stack);

                result = stack;
            }

            return result;
        }

        protected TItem TryUnStack(TItem item, int quantity)
        {
            var result = item;

            if (item.Quantity > quantity && HaveItem(item))
            {
                result = item.Clone();
                result.Quantity = quantity;

                item.Quantity -= quantity;
                ItemQuantityChanged?.Invoke(item);

                _items.TryAdd(result.UID, result);
                ItemAdded?.Invoke(result);
            }

            return result;
        }

        public bool HaveItem(TItem item) =>
            _items.ContainsKey(item.UID);

        public bool HaveItem(short itemTemplateId) =>
            _items.Values.FirstOrDefault(x => x.Id == itemTemplateId) is not null;

        public bool HaveItem(short itemTemplateId, int quantity) =>
            _items.Values.Where(x => x.Id == itemTemplateId).Sum(x => x.Quantity) >= quantity;

        public TItem? GetItemByUID(int uid) =>
            _items.ContainsKey(uid) ? _items[uid] : default;

        public TItem? GetItem(Predicate<TItem> p) =>
            _items.Values.FirstOrDefault(x => p(x));

        public TItem[] GetItems(Predicate<TItem>? p = default) =>
            (p is null ? _items.Values : _items.Values.Where(x => p(x))).ToArray();

        public TItem? AddItem(TItem item)
        {
            var result = default(TItem);

            if (item.Quantity > 0)
            {
                item.ChangeStorage((TStorage)this);

                result = TryStack(item);

                if (result is null && _items.TryAdd(item.UID, item))
                {
                    ItemAdded?.Invoke(item);

                    Refresh();

                    result = item;
                }
            }

            return result;
        }

        public void RemoveItem(TItem item, int quantity, bool delete = true)
        {
            if (quantity > 0 && HaveItem(item))
            {
                item.Quantity -= quantity;

                if (item.Quantity <= 0)
                {
                    ItemRemoved?.Invoke(item, delete);
                    _items.Remove(item.UID, out _);
                }
                else
                    ItemQuantityChanged?.Invoke(item);

                Refresh();
            }
        }

        public TItem? RemoveItem(int uid, int quantity, bool delete = true)
        {
            if (_items.TryGetValue(uid, out var item))
                RemoveItem(item, quantity, delete);

            return item;
        }

        public int DeleteItems(IEnumerable<TItem> items, int quantity)
        {
            var quantityToDelete = quantity;

            foreach (var item in items)
            {
                if (quantityToDelete is 0)
                    break;

                var quantityOfItemToDelete = item.Quantity > quantityToDelete ? quantityToDelete : item.Quantity;

                RemoveItem(item, quantityOfItemToDelete);

                quantityToDelete -= quantityOfItemToDelete;
            }

            return quantity - quantityToDelete;
        }

        public void ChangeItemOwner(TStorage storage, int uid, int quantity)
        {
            if (quantity > 0 && _items.TryGetValue(uid, out var item) && quantity <= item.Quantity)
            {
                item = TryUnStack(item, quantity);
                item.LastInteractionDate = DateTime.Now;

                storage.AddItem(item);
                RemoveItem(item, item.Quantity, false);
            }
        }

        public void MoveItemToCharacterInventory(Character character, int uid, int quantity)
        {
            if (quantity > 0)
            {
                if (_items.TryGetValue(uid, out var item) && quantity <= item.Quantity)
                {
                    var itemToMove = TryUnStack(item, quantity);

                    RemoveItem(itemToMove, itemToMove.Quantity, false);

                    character.Inventory.AddItem(ItemManager.Instance.CreatePlayerItem(new()
                    {
                        UID = itemToMove.UID,
                        ItemId = itemToMove.Id,
                        Effects = EffectManager.SerializeEffects(itemToMove.Effects),
                        Position = ItemManager.DefaultItemPosition,
                        Quantity = quantity,
                        LastInteractionDate = DateTime.Now
                    }, character.Inventory));
                }
                else
                    //Tu ne possèdes pas l\'objet nécessaire.
                    character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 4);
            }
        }

        public virtual void Refresh() { }

        public virtual void Save()
        {
            foreach (var itemToDelete in _itemsToDelete)
            {
                itemToDelete.Value.Key.Delete();

                if (itemToDelete.Value.Value)
                    ItemManager.Instance.IdProvider.Free(itemToDelete.Key);
            }
            _itemsToDelete.Clear();

            foreach (var item in _items)
                item.Value.Save(OwnerId);
        }
    }
}
