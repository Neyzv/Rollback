using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record PaddockPropertiesMessage : Message
    {
        public PaddockInformations properties;

        public const int Id = 5824;
        public override uint MessageId
        {
            get { return Id; }
        }
        public PaddockPropertiesMessage()
        {
        }
        public PaddockPropertiesMessage(PaddockInformations properties)
        {
            this.properties = properties;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(properties.TypeId);
            properties.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            properties = (PaddockInformations)ProtocolManager.Instance.GetType(reader.ReadUShort());
            properties.Deserialize(reader);
        }
    }
}
