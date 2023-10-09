using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record MountUnSetMessage : Message
	{
        public const int Id = 5982;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public MountUnSetMessage()
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
