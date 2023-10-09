using Rollback.World.Database.Characters;
using Rollback.World.Game.Items.Attributes;
using Rollback.World.Game.Items.Storages;

namespace Rollback.World.Game.Items.Types.Custom
{
    [ItemId(2239)]
    public sealed class EniripsaPowder : PlayerItem
    {
        public EniripsaPowder(CharacterItemRecord record, Inventory inventory)
            : base(record, inventory) { }

        public override bool Drop(PlayerItem dropOnItem)
        {
            if (dropOnItem is PetItem pet)
            {
                pet.LifePoints++;
                _storage.RefreshItem(pet.UID);
            }

            return true;
        }
    }
}
