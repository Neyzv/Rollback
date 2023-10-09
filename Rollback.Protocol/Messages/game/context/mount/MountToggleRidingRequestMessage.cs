using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record MountToggleRidingRequestMessage : Message
	{
        public const int Id = 5976;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public MountToggleRidingRequestMessage()
        {
        }
        public override void Serialize(BigEndianWriter writer)
        {
        }
        public override void Deserialize(BigEndianReader reader)
        {
		}
	}
}
