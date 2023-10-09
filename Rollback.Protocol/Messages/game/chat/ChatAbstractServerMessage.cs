using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record ChatAbstractServerMessage : Message
	{
        public sbyte channel;
        public string content;
        public int timestamp;
        public string fingerprint;

        public const int Id = 880;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ChatAbstractServerMessage()
        {
        }
        public ChatAbstractServerMessage(sbyte channel, string content, int timestamp, string fingerprint)
        {
            this.channel = channel;
            this.content = content;
            this.timestamp = timestamp;
            this.fingerprint = fingerprint;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(channel);
            writer.WriteString(content);
            writer.WriteInt(timestamp);
            writer.WriteString(fingerprint);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            channel = reader.ReadSByte();
            if (channel < 0)
                throw new Exception("Forbidden value on channel = " + channel + ", it doesn't respect the following condition : channel < 0");
            content = reader.ReadString();
            timestamp = reader.ReadInt();
            if (timestamp < 0)
                throw new Exception("Forbidden value on timestamp = " + timestamp + ", it doesn't respect the following condition : timestamp < 0");
            fingerprint = reader.ReadString();
		}
	}
}
