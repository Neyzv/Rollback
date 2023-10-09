using Rollback.Protocol.Enums;
using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Characters;
using Rollback.World.Game.Items.Storages;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.World.Maps;

namespace Rollback.World.Game.Items.Types
{
    public class PlayerItem : SaveableItem<PlayerItem, CharacterItemRecord, Character, Inventory>
    {
        public CharacterInventoryPositionEnum Position
        {
            get => (CharacterInventoryPositionEnum)_record.Position;
            set => _record.Position = (byte)value;
        }

        public bool IsEquipped =>
            Position is not CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED;

        public override bool CanStackOn(PlayerItem item) =>
            Position == item.Position && base.CanStackOn(item);

        public override ObjectItem ObjectItem
        {
            get
            {
                var objItem = base.ObjectItem;
                objItem.position = (byte)Position;

                return objItem;
            }
        }

        public PlayerItem(CharacterItemRecord record, Inventory inventory)
            : base(record, inventory) { }

        public event Action<PlayerItem>? Equipped;
        public void OnEquipped() =>
            Equipped?.Invoke(this);

        public event Action<PlayerItem>? Unequipped;
        public void OnUnequipped() =>
            Unequipped?.Invoke(this);

        public bool IsLinkedToCharacter() =>
            Effects.Any(x => x.Id is EffectId.EffectNonExchangeable981);

        public bool IsLinked() =>
            Effects.Any(x => x.Id is EffectId.EffectNonExchangeable981 || x.Id is EffectId.EffectNonExchangeable982);

        public virtual void UpdateItemSkin(Character owner)
        {
            if (AppearanceId > 0)
            {
                if (IsEquipped)
                    owner.CharacterLook.Skins.Add(AppearanceId);
                else
                    owner.CharacterLook.Skins.Remove(AppearanceId);

                owner.RefreshLook();
            }
        }

        public virtual bool Use(Cell? targetedCell) =>
            _record.Template!.Usable;

        public virtual bool Drop(PlayerItem dropOnItem) =>
            false;

        public virtual bool Feed(PlayerItem item) =>
            false;

        public override PlayerItem Clone() =>
            ItemManager.Instance.CreatePlayerItem(_record with
            {
                UID = ItemManager.Instance.IdProvider.Generate()
            }, _storage);
    }
}
