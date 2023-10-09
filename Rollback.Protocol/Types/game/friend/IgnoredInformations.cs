using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Types
{
    public record IgnoredInformations
    {
        public string name;
        public const short Id = 106;
        public virtual short TypeId
        {
            get { return Id; }
        }
        public IgnoredInformations()
        {
        }
        public IgnoredInformations(string name)
        {
            this.name = name;
        }
        public virtual void Serialize(BigEndianWriter writer)
        {
            writer.WriteString(name);
        }
        public virtual void Deserialize(BigEndianReader reader)
        {
            name = reader.ReadString();
        }
    }
}

