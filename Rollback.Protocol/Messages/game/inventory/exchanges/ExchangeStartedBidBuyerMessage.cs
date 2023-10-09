using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record ExchangeStartedBidBuyerMessage : Message
	{
        public SellerBuyerDescriptor buyerDescriptor;

        public const int Id = 5904;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeStartedBidBuyerMessage()
        {
        }
        public ExchangeStartedBidBuyerMessage(SellerBuyerDescriptor buyerDescriptor)
        {
            this.buyerDescriptor = buyerDescriptor;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            buyerDescriptor.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            buyerDescriptor = new SellerBuyerDescriptor();
            buyerDescriptor.Deserialize(reader);
		}
	}
}
