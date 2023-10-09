using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record HousePropertiesMessage : Message
    {
        public HouseInformations properties;

        public const int Id = 5734;
        public override uint MessageId
        {
            get { return Id; }
        }
        public HousePropertiesMessage()
        {
        }
        public HousePropertiesMessage(HouseInformations properties)
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
            properties = (HouseInformations)ProtocolManager.Instance.GetType(reader.ReadUShort());
            properties.Deserialize(reader);
        }
    }
}
