using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record ItemNoMoreAvailableMessage : Message
	{
        public const int Id = 5769;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ItemNoMoreAvailableMessage()
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
