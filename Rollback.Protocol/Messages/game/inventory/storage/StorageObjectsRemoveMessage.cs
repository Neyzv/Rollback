using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record StorageObjectsRemoveMessage : Message
	{
        public int[] objectUIDList;

        public const int Id = 6035;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public StorageObjectsRemoveMessage()
        {
        }
        public StorageObjectsRemoveMessage(int[] objectUIDList)
        {
            this.objectUIDList = objectUIDList;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)objectUIDList.Length);
            foreach (var entry in objectUIDList)
            {
                 writer.WriteInt(entry);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            objectUIDList = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 objectUIDList[i] = reader.ReadInt();
            }
		}
	}
}
