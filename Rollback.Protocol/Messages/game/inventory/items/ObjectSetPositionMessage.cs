using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record ObjectSetPositionMessage : Message
	{
        public int objectUID;
        public byte position;
        public int quantity;

        public const int Id = 3021;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ObjectSetPositionMessage()
        {
        }
        public ObjectSetPositionMessage(int objectUID, byte position, int quantity)
        {
            this.objectUID = objectUID;
            this.position = position;
            this.quantity = quantity;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(objectUID);
            writer.WriteByte(position);
            writer.WriteInt(quantity);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            objectUID = reader.ReadInt();
            if (objectUID < 0)
                throw new Exception("Forbidden value on objectUID = " + objectUID + ", it doesn't respect the following condition : objectUID < 0");
            position = reader.ReadByte();
            if (position < 0 || position > 255)
                throw new Exception("Forbidden value on position = " + position + ", it doesn't respect the following condition : position < 0 || position > 255");
            quantity = reader.ReadInt();
            if (quantity < 0)
                throw new Exception("Forbidden value on quantity = " + quantity + ", it doesn't respect the following condition : quantity < 0");
		}
	}
}
