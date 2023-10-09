using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record ExchangeSellOkMessage : Message
	{
        public const int Id = 5792;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeSellOkMessage()
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
