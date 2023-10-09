using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record ExchangeSellMessage : Message
	{
        public int objectToSellId;
        public int quantity;

        public const int Id = 5778;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeSellMessage()
        {
        }
        public ExchangeSellMessage(int objectToSellId, int quantity)
        {
            this.objectToSellId = objectToSellId;
            this.quantity = quantity;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(objectToSellId);
            writer.WriteInt(quantity);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            objectToSellId = reader.ReadInt();
            if (objectToSellId < 0)
                throw new Exception("Forbidden value on objectToSellId = " + objectToSellId + ", it doesn't respect the following condition : objectToSellId < 0");
            quantity = reader.ReadInt();
            if (quantity < 0)
                throw new Exception("Forbidden value on quantity = " + quantity + ", it doesn't respect the following condition : quantity < 0");
		}
	}
}
