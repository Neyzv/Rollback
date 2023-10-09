using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record CharacterToRecolorInformation
    {
        public int id;
        public int[] colors;
        public const short Id = 212;
        public virtual short TypeId
        {
            get { return Id; }
        }
        public CharacterToRecolorInformation()
        {
        }
        public CharacterToRecolorInformation(int id, int[] colors)
        {
            this.id = id;
            this.colors = colors;
        }
        public virtual void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(id);
            writer.WriteUShort((ushort)colors.Length);
            foreach (var entry in colors)
            {
                writer.WriteInt(entry);
            }
        }
        public virtual void Deserialize(BigEndianReader reader)
        {
            id = reader.ReadInt();
            if (id < 0)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < 0");
            var limit = reader.ReadUShort();
            colors = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                colors[i] = reader.ReadInt();
            }
        }
    }
}
