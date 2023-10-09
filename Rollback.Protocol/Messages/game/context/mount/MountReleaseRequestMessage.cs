using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record MountReleaseRequestMessage : Message
	{
        public const int Id = 5980;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public MountReleaseRequestMessage()
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
