using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record ExchangeStartOkCraftMessage : Message
	{
        public const int Id = 5813;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeStartOkCraftMessage()
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
