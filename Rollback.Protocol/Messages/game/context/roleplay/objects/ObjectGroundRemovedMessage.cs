using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record ObjectGroundRemovedMessage : Message
	{
        public short cell;

        public const int Id = 3014;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ObjectGroundRemovedMessage()
        {
        }
        public ObjectGroundRemovedMessage(short cell)
        {
            this.cell = cell;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(cell);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            cell = reader.ReadShort();
            if (cell < 0 || cell > 559)
                throw new Exception("Forbidden value on cell = " + cell + ", it doesn't respect the following condition : cell < 0 || cell > 559");
		}
	}
}
