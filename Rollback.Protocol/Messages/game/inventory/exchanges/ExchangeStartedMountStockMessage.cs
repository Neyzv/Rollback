using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record ExchangeStartedMountStockMessage : Message
	{
        public ObjectItem[] objectsInfos;

        public const int Id = 5984;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeStartedMountStockMessage()
        {
        }
        public ExchangeStartedMountStockMessage(ObjectItem[] objectsInfos)
        {
            this.objectsInfos = objectsInfos;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)objectsInfos.Length);
            foreach (var entry in objectsInfos)
            {
                 entry.Serialize(writer);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            objectsInfos = new ObjectItem[limit];
            for (int i = 0; i < limit; i++)
            {
                 objectsInfos[i] = new ObjectItem();
                 objectsInfos[i].Deserialize(reader);
            }
		}
	}
}
