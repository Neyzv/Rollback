using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record MountRidingMessage : Message
	{
        public bool isRiding;

        public const int Id = 5967;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public MountRidingMessage()
        {
        }
        public MountRidingMessage(bool isRiding)
        {
            this.isRiding = isRiding;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteBoolean(isRiding);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            isRiding = reader.ReadBoolean();
		}
	}
}
