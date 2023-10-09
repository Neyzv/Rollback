using Rollback.Common.ORM;
using Rollback.World.Database.Items;

namespace Rollback.World.Game.Items
{
    public static class ItemBaseRelator
    {
        public const string GetUIDByType = "SELECT UID FROM {0}";
    }

    public abstract record ItemBaseRecord
    {
        public ItemBaseRecord() =>
            Effects = Array.Empty<byte>();

        [Key]
        public int UID { get; set; }

        public int OwnerId { get; set; }

        private short _itemId;
        public short ItemId
        {
            get => _itemId;
            set
            {
                _itemId = value;
                Template = ItemManager.Instance.GetTemplateRecordById(value);
            }
        }

        public int Quantity { get; set; }

        public byte[] Effects { get; set; }

        [Ignore]
        public ItemRecord? Template { get; private set; }

        public DateTime LastInteractionDate { get; set; }
    }
}
