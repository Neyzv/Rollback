using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record GameContextRemoveElementMessage : Message
	{
        public int id;

        public const int Id = 251;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameContextRemoveElementMessage()
        {
        }
        public GameContextRemoveElementMessage(int id)
        {
            this.id = id;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(id);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            id = reader.ReadInt();
		}
	}
}
