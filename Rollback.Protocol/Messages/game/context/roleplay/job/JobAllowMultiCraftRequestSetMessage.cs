using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record JobAllowMultiCraftRequestSetMessage : Message
	{
        public bool enabled;

        public const int Id = 5749;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public JobAllowMultiCraftRequestSetMessage()
        {
        }
        public JobAllowMultiCraftRequestSetMessage(bool enabled)
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
