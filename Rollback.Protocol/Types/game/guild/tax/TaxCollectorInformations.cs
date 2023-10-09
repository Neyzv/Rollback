using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record TaxCollectorInformations
    {
        public int uniqueId;
        public short firtNameId;
        public short lastNameId;
        public AdditionalTaxCollectorInformations additonalInformation;
        public short worldX;
        public short worldY;
        public short subAreaId;
        public sbyte state;
        public EntityLook look;
        public const short Id = 167;
        public virtual short TypeId
        {
            get { return Id; }
        }
        public TaxCollectorInformations()
        {
        }
        public TaxCollectorInformations(int uniqueId, short firtNameId, short lastNameId, AdditionalTaxCollectorInformations additonalInformation, short worldX, short worldY, short subAreaId, sbyte state, EntityLook look)
        {
            this.uniqueId = uniqueId;
            this.firtNameId = firtNameId;
            this.lastNameId = lastNameId;
            this.additonalInformation = additonalInformation;
            this.worldX = worldX;
            this.worldY = worldY;
            this.subAreaId = subAreaId;
            this.state = state;
            this.look = look;
        }
        public virtual void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(uniqueId);
            writer.WriteShort(firtNameId);
            writer.WriteShort(lastNameId);
            additonalInformation.Serialize(writer);
            writer.WriteShort(worldX);
            writer.WriteShort(worldY);
            writer.WriteShort(subAreaId);
            writer.WriteSByte(state);
            look.Serialize(writer);
        }
        public virtual void Deserialize(BigEndianReader reader)
        {
            uniqueId = reader.ReadInt();
            firtNameId = reader.ReadShort();
            if (firtNameId < 0)
                throw new Exception("Forbidden value on firtNameId = " + firtNameId + ", it doesn't respect the following condition : firtNameId < 0");
            lastNameId = reader.ReadShort();
            if (lastNameId < 0)
                throw new Exception("Forbidden value on lastNameId = " + lastNameId + ", it doesn't respect the following condition : lastNameId < 0");
            additonalInformation = new AdditionalTaxCollectorInformations();
            additonalInformation.Deserialize(reader);
            worldX = reader.ReadShort();
            if (worldX < -255 || worldX > 255)
                throw new Exception("Forbidden value on worldX = " + worldX + ", it doesn't respect the following condition : worldX < -255 || worldX > 255");
            worldY = reader.ReadShort();
            if (worldY < -255 || worldY > 255)
                throw new Exception("Forbidden value on worldY = " + worldY + ", it doesn't respect the following condition : worldY < -255 || worldY > 255");
            subAreaId = reader.ReadShort();
            if (subAreaId < 0)
                throw new Exception("Forbidden value on subAreaId = " + subAreaId + ", it doesn't respect the following condition : subAreaId < 0");
            state = reader.ReadSByte();
            look = new EntityLook();
            look.Deserialize(reader);
        }
    }
}
