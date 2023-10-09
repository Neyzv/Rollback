using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record PaddockMoveItemRequestMessage : Message
	{
        public short oldCellId;
        public short newCellId;

        public const int Id = 6052;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public PaddockMoveItemRequestMessage()
        {
        }
        public PaddockMoveItemRequestMessage(short oldCellId, short newCellId)
        {
            this.oldCellId = oldCellId;
            this.newCellId = newCellId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(oldCellId);
            writer.WriteShort(newCellId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            oldCellId = reader.ReadShort();
            if (oldCellId < 0 || oldCellId > 559)
                throw new Exception("Forbidden value on oldCellId = " + oldCellId + ", it doesn't respect the following condition : oldCellId < 0 || oldCellId > 559");
            newCellId = reader.ReadShort();
            if (newCellId < 0 || newCellId > 559)
                throw new Exception("Forbidden value on newCellId = " + newCellId + ", it doesn't respect the following condition : newCellId < 0 || newCellId > 559");
		}
	}
}
