using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record ExchangeStartedBidSellerMessage : Message
	{
        public SellerBuyerDescriptor sellerDescriptor;
        public ObjectItemToSellInBid[] objectsInfos;

        public const int Id = 5905;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeStartedBidSellerMessage()
        {
        }
        public ExchangeStartedBidSellerMessage(SellerBuyerDescriptor sellerDescriptor, ObjectItemToSellInBid[] objectsInfos)
        {
            this.sellerDescriptor = sellerDescriptor;
            this.objectsInfos = objectsInfos;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            sellerDescriptor.Serialize(writer);
            writer.WriteUShort((ushort)objectsInfos.Length);
            foreach (var entry in objectsInfos)
            {
                 entry.Serialize(writer);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            sellerDescriptor = new SellerBuyerDescriptor();
            sellerDescriptor.Deserialize(reader);
            var limit = reader.ReadUShort();
            objectsInfos = new ObjectItemToSellInBid[limit];
            for (int i = 0; i < limit; i++)
            {
                 objectsInfos[i] = new ObjectItemToSellInBid();
                 objectsInfos[i].Deserialize(reader);
            }
		}
	}
}
