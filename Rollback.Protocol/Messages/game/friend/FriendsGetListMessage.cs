using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record FriendsGetListMessage : Message
	{
        public const int Id = 4001;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public FriendsGetListMessage()
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
