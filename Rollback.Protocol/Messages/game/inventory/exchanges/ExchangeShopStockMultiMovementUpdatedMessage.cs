using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record ExchangeShopStockMultiMovementUpdatedMessage : Message
	{
        public ObjectItemToSell[] objectInfoList;

        public const int Id = 6038;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeShopStockMultiMovementUpdatedMessage()
        {
        }
        public ExchangeShopStockMultiMovementUpdatedMessage(ObjectItemToSell[] objectInfoList)
        {
            this.objectInfoList = objectInfoList;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)objectInfoList.Length);
            foreach (var entry in objectInfoList)
            {
                 entry.Serialize(writer);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            objectInfoList = new ObjectItemToSell[limit];
            for (int i = 0; i < limit; i++)
            {
                 objectInfoList[i] = new ObjectItemToSell();
                 objectInfoList[i].Deserialize(reader);
            }
		}
	}
}
