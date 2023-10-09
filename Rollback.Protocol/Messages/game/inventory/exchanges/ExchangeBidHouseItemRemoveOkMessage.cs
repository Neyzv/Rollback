using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record ExchangeBidHouseItemRemoveOkMessage : Message
	{
        public int sellerId;

        public const int Id = 5946;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeBidHouseItemRemoveOkMessage()
        {
        }
        public ExchangeBidHouseItemRemoveOkMessage(int sellerId)
        {
            this.sellerId = sellerId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(sellerId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            sellerId = reader.ReadInt();
		}
	}
}
