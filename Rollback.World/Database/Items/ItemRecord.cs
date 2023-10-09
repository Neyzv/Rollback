using Rollback.Common.Logging;
using Rollback.Common.ORM;
using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Criterion;
using Rollback.World.Game.Effects;
using Rollback.World.Game.Items;

namespace Rollback.World.Database.Items
{
    public static class ItemRelator
    {
        public const string GetItems = "SELECT * FROM items_templates";
    }

    [Table("items_templates")]
    public sealed record ItemRecord
    {
        public ItemRecord() =>
            (_stringCriterion, _binaryEffects, Effects, RecipesCSV) = (string.Empty, Array.Empty<byte>(), Array.Empty<EffectBase>(), string.Empty);

        [Key]
        public short Id { get; set; }

        public ItemType TypeId { get; set; }

        public short Level { get; set; }

        public int Weight { get; set; }

        public bool Usable { get; set; }

        public bool Targetable { get; set; }

        public bool Etheral { get; set; }

        public int Price { get; set; }

        private short _itemSetId;
        public short ItemSetId
        {
            get => _itemSetId;
            set
            {
                _itemSetId = value;
                Set = ItemManager.Instance.GetSetRecordById(value);
            }
        }

        [Ignore]
        public ItemSetRecord? Set { get; set; }

        private string _stringCriterion;
        public string StringCriterion
        {
            get => _stringCriterion;
            set
            {
                _stringCriterion = value;

                if (!string.IsNullOrEmpty(value))
                    Criterion = CriterionManager.Instance.Parse(value);
            }
        }

        [Ignore]
        public CriterionExpression? Criterion { get; private set; }

        public short AppearanceId { get; set; }

        private byte[] _binaryEffects;
        public byte[] BinaryEffects
        {
            get => _binaryEffects;
            set
            {
                _binaryEffects = value;
                try
                {
                    Effects = EffectManager.DeserializeEffects(value).ToArray();
                }
                catch
                {
                    Logger.Instance.LogError(msg: $"Incorrect datas format while deserializing effects of item {Id}...");
                }
            }
        }

        [Ignore]
        public EffectBase[] Effects { get; private set; }

        public string RecipesCSV { get; set; }

        public bool TwoHanded { get; set; }

        public short APCost { get; set; }

        public sbyte MinRange { get; set; }

        public sbyte MaxRange { get; set; }

        public bool CastInLine { get; set; }

        public bool CastTestLOS { get; set; }

        public sbyte CriticalHitProbability { get; set; }

        public sbyte CriticalHitBonus { get; set; }

        public sbyte CriticalFailureProbability { get; set; }

        [Ignore]
        public ObjectItemMinimalInformation ObjectItemMinimalInformation =>
            new(Id, EffectManager.GetObjectEffects(Effects));
    }
}
