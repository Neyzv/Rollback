using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record ExchangeStartOkNpcShopMessage : Message
	{
        public int npcSellerId;
        public ObjectItemToSellInNpcShop[] objectsInfos;

        public const int Id = 5761;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeStartOkNpcShopMessage()
        {
        }
        public ExchangeStartOkNpcShopMessage(int npcSellerId, ObjectItemToSellInNpcShop[] objectsInfos)
        {
            this.npcSellerId = npcSellerId;
            this.objectsInfos = objectsInfos;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(npcSellerId);
            writer.WriteUShort((ushort)objectsInfos.Length);
            foreach (var entry in objectsInfos)
            {
                 entry.Serialize(writer);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            npcSellerId = reader.ReadInt();
            var limit = reader.ReadUShort();
            objectsInfos = new ObjectItemToSellInNpcShop[limit];
            for (int i = 0; i < limit; i++)
            {
                 objectsInfos[i] = new ObjectItemToSellInNpcShop();
                 objectsInfos[i].Deserialize(reader);
            }
		}
	}
}
