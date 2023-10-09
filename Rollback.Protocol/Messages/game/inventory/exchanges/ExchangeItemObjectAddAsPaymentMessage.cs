using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record ExchangeItemObjectAddAsPaymentMessage : Message
	{
        public sbyte paymentType;
        public bool bAdd;
        public int objectToMoveId;
        public int quantity;

        public const int Id = 5766;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeItemObjectAddAsPaymentMessage()
        {
        }
        public ExchangeItemObjectAddAsPaymentMessage(sbyte paymentType, bool bAdd, int objectToMoveId, int quantity)
        {
            this.paymentType = paymentType;
            this.bAdd = bAdd;
            this.objectToMoveId = objectToMoveId;
            this.quantity = quantity;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(paymentType);
            writer.WriteBoolean(bAdd);
            writer.WriteInt(objectToMoveId);
            writer.WriteInt(quantity);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            paymentType = reader.ReadSByte();
            bAdd = reader.ReadBoolean();
            objectToMoveId = reader.ReadInt();
            if (objectToMoveId < 0)
                throw new Exception("Forbidden value on objectToMoveId = " + objectToMoveId + ", it doesn't respect the following condition : objectToMoveId < 0");
            quantity = reader.ReadInt();
            if (quantity < 0)
                throw new Exception("Forbidden value on quantity = " + quantity + ", it doesn't respect the following condition : quantity < 0");
		}
	}
}
