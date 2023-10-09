using Rollback.Common.ORM;
using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Criterion;
using Rollback.World.Game.Effects;

namespace Rollback.World.Game.Items
{
    public abstract class SaveableItem<TItem, TRecord, TOwner, TStorage>
        where TItem : SaveableItem<TItem, TRecord, TOwner, TStorage>
        where TStorage : SaveableItemsStorage<TOwner, TRecord, TItem, TStorage>
        where TRecord : ItemBaseRecord
        where TOwner : class
    {
        protected readonly TRecord _record;
        protected TStorage _storage;

        public int UID
        {
            get => _record.UID;
            set => _record.UID = value;
        }

        public List<EffectBase> Effects { get; set; }

        public int Quantity
        {
            get => _record.Quantity;
            set => _record.Quantity = value;
        }

        public DateTime LastInteractionDate
        {
            get => _record.LastInteractionDate;
            set => _record.LastInteractionDate = value;
        }

        public short Id =>
            _record.Template!.Id;

        public short AppearanceId =>
            _record.Template!.AppearanceId;

        public ItemType TypeId =>
            _record.Template!.TypeId;

        public short ItemSetId =>
            _record.Template!.ItemSetId;

        public IReadOnlyCollection<IReadOnlyCollection<EffectBase>>? SetEffects =>
            _record.Template!.Set?.Effects;

        public short Level =>
            _record.Template!.Level;

        public int Weight =>
            _record.Template!.Weight;

        public int Price =>
            _record.Template!.Price;

        public bool TwoHanded =>
            _record.Template!.TwoHanded;

        public CriterionExpression? Criterion =>
            _record.Template!.Criterion;

        public bool Targetable =>
            _record.Template!.Targetable;

        public bool IsWeapon =>
            TypeId is ItemType.Outil or ItemType.Faux or ItemType.Hache or
            ItemType.Marteau or ItemType.Dague or ItemType.Baguette or
            ItemType.Baton or ItemType.Arc or ItemType.Pelle or
            ItemType.PierreDAme or ItemType.Pioche or ItemType.Epee;

        public virtual ObjectItem ObjectItem =>
            new(ItemManager.DefaultItemPosition, Id, EffectManager.GetObjectEffects(Effects), UID, Quantity);

        public ObjectItemMinimalInformation ObjectItemMinimalInformation =>
            new(Id, EffectManager.GetObjectEffects(Effects));

        public SaveableItem(TRecord record, TStorage storage)
        {
            _record = record;
            _storage = storage;
            Effects = EffectManager.DeserializeEffects(record.Effects);
        }

        public event Action<TItem>? Created;
        public void OnCreated() =>
            Created?.Invoke((TItem)this);

        public event Action<TItem>? Deleted;
        public void OnDeleted() =>
            Deleted?.Invoke((TItem)this);

        public event Action<TItem>? BeforeSaved;

        public virtual bool CanStackOn(TItem item) =>
            Id == item.Id && Effects.Count == item.Effects.Count && Effects.All(x => item.Effects.Contains(x));

        public virtual TItem Clone()
        {
            return (TItem)Activator.CreateInstance(typeof(TItem), new[] {_record with
            {
                UID = ItemManager.Instance.IdProvider.Generate(),
                LastInteractionDate = DateTime.Now
            }})!;
        }

        public void ChangeStorage(TStorage newStorage)
        {
            _storage.RemoveItem(UID, Quantity, false);
            _storage = newStorage;
        }

        public void Save(int ownerId)
        {
            BeforeSaved?.Invoke((TItem)this);

            _record.OwnerId = ownerId;
            _record.Effects = EffectManager.SerializeEffects(Effects);

            DatabaseAccessor.Instance.InsertOrUpdate(_record);
        }

        public void Delete() =>
            DatabaseAccessor.Instance.Delete(_record);
    }
}
