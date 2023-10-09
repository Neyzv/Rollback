using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record ExchangeBidSearchOkMessage : Message
	{
        public const int Id = 5802;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeBidSearchOkMessage()
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
