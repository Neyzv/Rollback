using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record CharacterMinimalInformations
    {
        public int id;
        public string name;
        public byte level;
        public const short Id = 110;
        public virtual short TypeId
        {
            get { return Id; }
        }
        public CharacterMinimalInformations()
        {
        }
        public CharacterMinimalInformations(int id, string name, byte level)
        {
            this.id = id;
            this.name = name;
            this.level = level;
        }
        public virtual void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(id);
            writer.WriteString(name);
            writer.WriteByte(level);
        }
        public virtual void Deserialize(BigEndianReader reader)
        {
            id = reader.ReadInt();
            if (id < 0)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < 0");
            name = reader.ReadString();
            level = reader.ReadByte();
            if (level < 1 || level > 200)
                throw new Exception("Forbidden value on level = " + level + ", it doesn't respect the following condition : level < 1 || level > 200");
        }
    }
}