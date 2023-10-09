using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record MonsterInGroupInformations
    {
        public int creatureGenericId;
        public short level;
        public EntityLook look;
        public const short Id = 144;
        public virtual short TypeId
        {
            get { return Id; }
        }
        public MonsterInGroupInformations()
        {
        }
        public MonsterInGroupInformations(int creatureGenericId, short level, EntityLook look)
        {
            this.creatureGenericId = creatureGenericId;
            this.level = level;
            this.look = look;
        }
        public virtual void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(creatureGenericId);
            writer.WriteShort(level);
            look.Serialize(writer);
        }
        public virtual void Deserialize(BigEndianReader reader)
        {
            creatureGenericId = reader.ReadInt();
            level = reader.ReadShort();
            if (level < 0)
                throw new Exception("Forbidden value on level = " + level + ", it doesn't respect the following condition : level < 0");
            look = new EntityLook();
            look.Deserialize(reader);
        }
    }
}
