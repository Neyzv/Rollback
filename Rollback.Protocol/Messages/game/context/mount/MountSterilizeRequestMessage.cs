using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record MountSterilizeRequestMessage : Message
	{
        public const int Id = 5962;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public MountSterilizeRequestMessage()
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
