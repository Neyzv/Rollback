using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record TaxCollectorName
    {
        public short firstNameId;
        public short lastNameId;
        public const short Id = 187;
        public virtual short TypeId
        {
            get { return Id; }
        }
        public TaxCollectorName()
        {
        }
        public TaxCollectorName(short firstNameId, short lastNameId)
        {
            this.firstNameId = firstNameId;
            this.lastNameId = lastNameId;
        }
        public virtual void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(firstNameId);
            writer.WriteShort(lastNameId);
        }
        public virtual void Deserialize(BigEndianReader reader)
        {
            firstNameId = reader.ReadShort();
            if (firstNameId < 0)
                throw new Exception("Forbidden value on firstNameId = " + firstNameId + ", it doesn't respect the following condition : firstNameId < 0");
            lastNameId = reader.ReadShort();
            if (lastNameId < 0)
                throw new Exception("Forbidden value on lastNameId = " + lastNameId + ", it doesn't respect the following condition : lastNameId < 0");
        }
    }
}