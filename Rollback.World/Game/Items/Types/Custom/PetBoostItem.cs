using Rollback.Protocol.Enums;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Characters;
using Rollback.World.Game.Items.Attributes;
using Rollback.World.Game.Items.Storages;
using Rollback.World.Game.World.Maps;

namespace Rollback.World.Game.Items.Types.Custom
{
    [ItemType((short)ItemType.PotionDeFamilier)]
    internal sealed class PetBoostItem : PlayerItem
    {
        public PetBoostItem(CharacterItemRecord record, Inventory inventory)
            : base(record, inventory) { }

        public override bool Use(Cell? targetedCell) =>
            _storage.GetEquipedItem(CharacterInventoryPositionEnum.ACCESSORY_POSITION_PETS) is PetItem pet &&
            pet.BoostItemId == Id && pet.Boost();
    }
}