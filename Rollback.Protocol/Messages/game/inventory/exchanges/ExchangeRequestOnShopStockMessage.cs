using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record ExchangeRequestOnShopStockMessage : Message
	{
        public const int Id = 5753;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeRequestOnShopStockMessage()
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
