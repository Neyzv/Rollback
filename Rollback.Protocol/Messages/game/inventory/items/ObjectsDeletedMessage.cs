using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record ObjectsDeletedMessage : Message
	{
        public int[] objectUID;

        public const int Id = 6034;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ObjectsDeletedMessage()
        {
        }
        public ObjectsDeletedMessage(int[] objectUID)
        {
            this.objectUID = objectUID;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)objectUID.Length);
            foreach (var entry in objectUID)
            {
                 writer.WriteInt(entry);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            objectUID = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 objectUID[i] = reader.ReadInt();
            }
		}
	}
}
