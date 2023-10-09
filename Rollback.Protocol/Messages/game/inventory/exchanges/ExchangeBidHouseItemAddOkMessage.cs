using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record ExchangeBidHouseItemAddOkMessage : Message
	{
        public ObjectItemToSellInBid itemInfo;

        public const int Id = 5945;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeBidHouseItemAddOkMessage()
        {
        }
        public ExchangeBidHouseItemAddOkMessage(ObjectItemToSellInBid itemInfo)
        {
            this.itemInfo = itemInfo;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            itemInfo.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            itemInfo = new ObjectItemToSellInBid();
            itemInfo.Deserialize(reader);
		}
	}
}
