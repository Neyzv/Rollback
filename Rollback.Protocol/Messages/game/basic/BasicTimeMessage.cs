using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record BasicTimeMessage : Message
	{
        public int timestamp;
        public short timezoneOffset;

        public const int Id = 175;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public BasicTimeMessage()
        {
        }
        public BasicTimeMessage(int timestamp, short timezoneOffset)
        {
            this.timestamp = timestamp;
            this.timezoneOffset = timezoneOffset;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(timestamp);
            writer.WriteShort(timezoneOffset);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            timestamp = reader.ReadInt();
            if (timestamp < 0)
                throw new Exception("Forbidden value on timestamp = " + timestamp + ", it doesn't respect the following condition : timestamp < 0");
            timezoneOffset = reader.ReadShort();
		}
	}
}
