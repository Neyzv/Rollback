using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Types
{
    public record MountInformationsForPaddock
    {
        public int modelId;
        public string name;
        public string ownerName;
        public const short Id = 184;
        public virtual short TypeId
        {
            get { return Id; }
        }
        public MountInformationsForPaddock()
        {
        }
        public MountInformationsForPaddock(int modelId, string name, string ownerName)
        {
            this.modelId = modelId;
            this.name = name;
            this.ownerName = ownerName;
        }
        public virtual void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(modelId);
            writer.WriteString(name);
            writer.WriteString(ownerName);
        }
        public virtual void Deserialize(BigEndianReader reader)
        {
            modelId = reader.ReadInt();
            name = reader.ReadString();
            ownerName = reader.ReadString();
        }
    }
}