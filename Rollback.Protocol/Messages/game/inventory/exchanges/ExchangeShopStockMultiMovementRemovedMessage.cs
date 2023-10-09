using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record ExchangeShopStockMultiMovementRemovedMessage : Message
	{
        public int[] objectIdList;

        public const int Id = 6037;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeShopStockMultiMovementRemovedMessage()
        {
        }
        public ExchangeShopStockMultiMovementRemovedMessage(int[] objectIdList)
        {
            this.objectIdList = objectIdList;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)objectIdList.Length);
            foreach (var entry in objectIdList)
            {
                 writer.WriteInt(entry);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            objectIdList = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 objectIdList[i] = reader.ReadInt();
            }
		}
	}
}
