using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record ExchangeObjectMoveMessage : Message
	{
        public int objectUID;
        public short quantity;

        public const int Id = 5518;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeObjectMoveMessage()
        {
        }
        public ExchangeObjectMoveMessage(int objectUID, short quantity)
        {
            this.objectUID = objectUID;
            this.quantity = quantity;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(objectUID);
            writer.WriteShort(quantity);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            objectUID = reader.ReadInt();
            if (objectUID < 0)
                throw new Exception("Forbidden value on objectUID = " + objectUID + ", it doesn't respect the following condition : objectUID < 0");
            quantity = reader.ReadShort();
		}
	}
}
