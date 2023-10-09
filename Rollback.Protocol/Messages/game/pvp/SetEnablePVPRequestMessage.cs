using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record SetEnablePVPRequestMessage : Message
	{
        public bool enable;

        public const int Id = 1810;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public SetEnablePVPRequestMessage()
        {
        }
        public SetEnablePVPRequestMessage(bool enable)
        {
            this.enable = enable;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteBoolean(enable);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            enable = reader.ReadBoolean();
		}
	}
}
