using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record QueueStatusMessage : Message
	{
        public ushort position;
        public ushort total;

        public const int Id = 6100;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public QueueStatusMessage()
        {
        }
        public QueueStatusMessage(ushort position, ushort total)
        {
            this.position = position;
            this.total = total;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort(position);
            writer.WriteUShort(total);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            position = reader.ReadUShort();
            if (position < 0 || position > 65535)
                throw new Exception("Forbidden value on position = " + position + ", it doesn't respect the following condition : position < 0 || position > 65535");
            total = reader.ReadUShort();
            if (total < 0 || total > 65535)
                throw new Exception("Forbidden value on total = " + total + ", it doesn't respect the following condition : total < 0 || total > 65535");
		}
	}
}
