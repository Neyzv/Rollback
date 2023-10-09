using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record ExchangeShopStockMovementUpdatedMessage : Message
	{
        public ObjectItemToSell objectInfo;

        public const int Id = 5909;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeShopStockMovementUpdatedMessage()
        {
        }
        public ExchangeShopStockMovementUpdatedMessage(ObjectItemToSell objectInfo)
        {
            this.objectInfo = objectInfo;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            objectInfo.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            objectInfo = new ObjectItemToSell();
            objectInfo.Deserialize(reader);
		}
	}
}
