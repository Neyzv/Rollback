using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record ExchangeObjectMoveKamaMessage : Message
	{
        public int quantity;

        public const int Id = 5520;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeObjectMoveKamaMessage()
        {
        }
        public ExchangeObjectMoveKamaMessage(int quantity)
        {
            this.quantity = quantity;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(quantity);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            quantity = reader.ReadInt();
		}
	}
}
