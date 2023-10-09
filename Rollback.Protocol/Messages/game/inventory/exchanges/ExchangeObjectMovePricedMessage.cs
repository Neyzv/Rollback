using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record ExchangeObjectMovePricedMessage : ExchangeObjectMoveMessage
	{
        public int price;

        public new const int Id = 5514;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeObjectMovePricedMessage()
        {
        }
        public ExchangeObjectMovePricedMessage(int objectUID, short quantity, int price) : base(objectUID, quantity)
        {
            this.price = price;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(price);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            price = reader.ReadInt();
		}
	}
}
