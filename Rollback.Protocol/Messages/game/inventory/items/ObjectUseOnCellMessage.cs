using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record ObjectUseOnCellMessage : ObjectUseMessage
	{
        public short cells;

        public new const int Id = 3013;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ObjectUseOnCellMessage()
        {
        }
        public ObjectUseOnCellMessage(int objectUID, short cells) : base(objectUID)
        {
            this.cells = cells;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(cells);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            cells = reader.ReadShort();
            if (cells < 0 || cells > 559)
                throw new Exception("Forbidden value on cells = " + cells + ", it doesn't respect the following condition : cells < 0 || cells > 559");
		}
	}
}
