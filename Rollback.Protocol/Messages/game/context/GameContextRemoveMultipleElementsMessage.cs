using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record GameContextRemoveMultipleElementsMessage : Message
	{
        public int[] id;

        public const int Id = 252;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameContextRemoveMultipleElementsMessage()
        {
        }
        public GameContextRemoveMultipleElementsMessage(int[] id)
        {
            this.id = id;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)id.Length);
            foreach (var entry in id)
            {
                 writer.WriteInt(entry);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            id = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 id[i] = reader.ReadInt();
            }
		}
	}
}
