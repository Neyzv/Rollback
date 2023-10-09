using Rollback.Common.ORM;
using Rollback.World.Game.Effects;

namespace Rollback.World.Database.Items
{
    public static class ItemSetRelator
    {
        public const string GetSets = "SELECT * FROM items_sets";
    }

    [Table("items_sets")]
    public sealed record ItemSetRecord
    {
        public ItemSetRecord() =>
            (_itemsCSV, Items, _binaryEffects, _effects) = (string.Empty, Array.Empty<int>(), Array.Empty<byte>(), new());

        [Key]
        public short Id { get; set; }

        private string _itemsCSV;
        public string ItemsCSV
        {
            get => _itemsCSV;
            set
            {
                _itemsCSV = value;
                Items = value.Split(",").Select(x => int.Parse(x)).ToArray();
            }
        }

        [Ignore]
        public int[] Items { get; private set; }

        private byte[] _binaryEffects;
        public byte[] BinaryEffects
        {
            get => _binaryEffects;
            set
            {
                _binaryEffects = value;
                _effects = EffectManager.DeserializeSetEffects(value);
            }
        }

        private List<List<EffectBase>> _effects;
        [Ignore]
        public List<List<EffectBase>> Effects
        {
            get => _effects;
            set
            {
                _effects = value;
                _binaryEffects = EffectManager.SerializeSetEffects(value);
            }
        }
    }
}
