using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record PaddockSellRequestMessage : Message
	{
        public int price;

        public const int Id = 5953;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public PaddockSellRequestMessage()
        {
        }
        public PaddockSellRequestMessage(int price)
        {
            this.price = price;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(price);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            price = reader.ReadInt();
            if (price < 0)
                throw new Exception("Forbidden value on price = " + price + ", it doesn't respect the following condition : price < 0");
		}
	}
}
