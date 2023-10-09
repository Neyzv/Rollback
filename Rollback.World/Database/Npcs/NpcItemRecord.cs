using Rollback.Common.ORM;
using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Items;
using Rollback.World.Game.Effects;
using Rollback.World.Game.Items;

namespace Rollback.World.Database.Npcs
{
    public static class NpcItemRelator
    {
        public const string GetItemsByActionId = "SELECT * FROM npcs_items WHERE ShopActionId = {0}";
    }

    [Table("npcs_items")]
    public sealed record NpcItemRecord
    {
        [Key]
        public int Id { get; set; }

        public int ShopActionId { get; set; }

        private short _itemId;
        public short ItemId
        {
            get => _itemId;
            set
            {
                _itemId = value;
                ItemRecord = ItemManager.Instance.GetTemplateRecordById(value);
            }
        }

        [Ignore]
        public ItemRecord? ItemRecord { get; private set; }

        private int? _price;
        public int Price
        {
            get => _price ??= ItemRecord!.Price;
            set => _price = value;
        }

        public EffectGenerationType EffectGenerationType { get; set; }

        [Ignore]
        public ObjectItemToSellInNpcShop ObjectItemToSellInNpcShop =>
            new(ItemId, EffectManager.GetObjectEffects(ItemRecord!.Effects), Price, string.Empty);
    }
}
