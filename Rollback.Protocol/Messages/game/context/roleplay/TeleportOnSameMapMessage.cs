using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record TeleportOnSameMapMessage : Message
	{
        public int targetId;
        public short cellId;

        public const int Id = 6048;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public TeleportOnSameMapMessage()
        {
        }
        public TeleportOnSameMapMessage(int targetId, short cellId)
        {
            this.targetId = targetId;
            this.cellId = cellId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(targetId);
            writer.WriteShort(cellId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            targetId = reader.ReadInt();
            cellId = reader.ReadShort();
            if (cellId < 0 || cellId > 559)
                throw new Exception("Forbidden value on cellId = " + cellId + ", it doesn't respect the following condition : cellId < 0 || cellId > 559");
		}
	}
}
