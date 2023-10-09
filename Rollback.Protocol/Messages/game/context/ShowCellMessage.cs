using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record ShowCellMessage : Message
	{
        public int sourceId;
        public short cellId;

        public const int Id = 5612;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ShowCellMessage()
        {
        }
        public ShowCellMessage(int sourceId, short cellId)
        {
            this.sourceId = sourceId;
            this.cellId = cellId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(sourceId);
            writer.WriteShort(cellId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            sourceId = reader.ReadInt();
            cellId = reader.ReadShort();
            if (cellId < 0 || cellId > 559)
                throw new Exception("Forbidden value on cellId = " + cellId + ", it doesn't respect the following condition : cellId < 0 || cellId > 559");
		}
	}
}
