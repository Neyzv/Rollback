using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record TaxCollectorBasicInformations
    {
        public short firstNameId;
        public short lastNameId;
        public int mapId;
        public const short Id = 96;
        public virtual short TypeId
        {
            get { return Id; }
        }
        public TaxCollectorBasicInformations()
        {
        }
        public TaxCollectorBasicInformations(short firstNameId, short lastNameId, int mapId)
        {
            this.firstNameId = firstNameId;
            this.lastNameId = lastNameId;
            this.mapId = mapId;
        }
        public virtual void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(firstNameId);
            writer.WriteShort(lastNameId);
            writer.WriteInt(mapId);
        }
        public virtual void Deserialize(BigEndianReader reader)
        {
            firstNameId = reader.ReadShort();
            if (firstNameId < 0)
                throw new Exception("Forbidden value on firstNameId = " + firstNameId + ", it doesn't respect the following condition : firstNameId < 0");
            lastNameId = reader.ReadShort();
            if (lastNameId < 0)
                throw new Exception("Forbidden value on lastNameId = " + lastNameId + ", it doesn't respect the following condition : lastNameId < 0");
            mapId = reader.ReadInt();
        }
    }
}

		