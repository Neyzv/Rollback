using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record SubscriptionZoneMessage : Message
	{
        public bool active;

        public const int Id = 5573;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public SubscriptionZoneMessage()
        {
        }
        public SubscriptionZoneMessage(bool active)
        {
            this.active = active;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteBoolean(active);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            active = reader.ReadBoolean();
		}
	}
}
