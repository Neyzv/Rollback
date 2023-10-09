using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record AlreadyConnectedMessage : Message
	{
        public const int Id = 109;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public AlreadyConnectedMessage()
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
