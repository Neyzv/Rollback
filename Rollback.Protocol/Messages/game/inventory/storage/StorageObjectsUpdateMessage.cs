using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record StorageObjectsUpdateMessage : Message
	{
        public ObjectItem[] objectList;

        public const int Id = 6036;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public StorageObjectsUpdateMessage()
        {
        }
        public StorageObjectsUpdateMessage(ObjectItem[] objectList)
        {
            this.objectList = objectList;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)objectList.Length);
            foreach (var entry in objectList)
            {
                 entry.Serialize(writer);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            objectList = new ObjectItem[limit];
            for (int i = 0; i < limit; i++)
            {
                 objectList[i] = new ObjectItem();
                 objectList[i].Deserialize(reader);
            }
		}
	}
}
