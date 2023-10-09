using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record ExchangeBidHouseBuyMessage : Message
	{
        public int uid;
        public int qty;
        public int price;

        public const int Id = 5804;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeBidHouseBuyMessage()
        {
        }
        public ExchangeBidHouseBuyMessage(int uid, int qty, int price)
        {
            this.uid = uid;
            this.qty = qty;
            this.price = price;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(uid);
            writer.WriteInt(qty);
            writer.WriteInt(price);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            uid = reader.ReadInt();
            if (uid < 0)
                throw new Exception("Forbidden value on uid = " + uid + ", it doesn't respect the following condition : uid < 0");
            qty = reader.ReadInt();
            if (qty < 0)
                throw new Exception("Forbidden value on qty = " + qty + ", it doesn't respect the following condition : qty < 0");
            price = reader.ReadInt();
            if (price < 0)
                throw new Exception("Forbidden value on price = " + price + ", it doesn't respect the following condition : price < 0");
		}
	}
}
