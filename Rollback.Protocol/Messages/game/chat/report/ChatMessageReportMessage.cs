using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record ChatMessageReportMessage : Message
	{
        public string senderName;
        public string content;
        public int timestamp;
        public string fingerprint;
        public sbyte reason;

        public const int Id = 821;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ChatMessageReportMessage()
        {
        }
        public ChatMessageReportMessage(string senderName, string content, int timestamp, string fingerprint, sbyte reason)
        {
            this.senderName = senderName;
            this.content = content;
            this.timestamp = timestamp;
            this.fingerprint = fingerprint;
            this.reason = reason;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteString(senderName);
            writer.WriteString(content);
            writer.WriteInt(timestamp);
            writer.WriteString(fingerprint);
            writer.WriteSByte(reason);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            senderName = reader.ReadString();
            content = reader.ReadString();
            timestamp = reader.ReadInt();
            if (timestamp < 0)
                throw new Exception("Forbidden value on timestamp = " + timestamp + ", it doesn't respect the following condition : timestamp < 0");
            fingerprint = reader.ReadString();
            reason = reader.ReadSByte();
            if (reason < 0)
                throw new Exception("Forbidden value on reason = " + reason + ", it doesn't respect the following condition : reason < 0");
		}
	}
}
