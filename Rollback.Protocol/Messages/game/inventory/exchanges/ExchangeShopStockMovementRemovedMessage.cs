using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record ExchangeShopStockMovementRemovedMessage : Message
	{
        public int objectId;

        public const int Id = 5907;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeShopStockMovementRemovedMessage()
        {
        }
        public ExchangeShopStockMovementRemovedMessage(int objectId)
        {
            this.objectId = objectId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(objectId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            objectId = reader.ReadInt();
            if (objectId < 0)
                throw new Exception("Forbidden value on objectId = " + objectId + ", it doesn't respect the following condition : objectId < 0");
		}
	}
}
