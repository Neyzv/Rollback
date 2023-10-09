using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record HouseExitedMessage : Message
	{
        public const int Id = 5861;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public HouseExitedMessage()
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
