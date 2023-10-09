using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Types
{
    public record ItemDurability
    {
        public short durability;
        public short durabilityMax;
        public const short Id = 168;
        public virtual short TypeId
        {
            get { return Id; }
        }
        public ItemDurability()
        {
        }
        public ItemDurability(short durability, short durabilityMax)
        {
            this.durability = durability;
            this.durabilityMax = durabilityMax;
        }
        public virtual void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(durability);
            writer.WriteShort(durabilityMax);
        }
        public virtual void Deserialize(BigEndianReader reader)
        {
            durability = reader.ReadShort();
            durabilityMax = reader.ReadShort();
        }
    }
}