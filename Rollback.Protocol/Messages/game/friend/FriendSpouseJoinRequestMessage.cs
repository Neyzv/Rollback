using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record FriendSpouseJoinRequestMessage : Message
	{
        public const int Id = 5604;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public FriendSpouseJoinRequestMessage()
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
