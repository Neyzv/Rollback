using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record JobAllowMultiCraftRequestMessage : Message
	{
        public bool enabled;

        public const int Id = 5748;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public JobAllowMultiCraftRequestMessage()
        {
        }
        public JobAllowMultiCraftRequestMessage(bool enabled)
        {
            this.enabled = enabled;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteBoolean(enabled);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            enabled = reader.ReadBoolean();
		}
	}
}
