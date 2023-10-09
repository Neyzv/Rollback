using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record ChatServerMessage : ChatAbstractServerMessage
	{
        public int senderId;
        public string senderName;

        public new const int Id = 881;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ChatServerMessage()
        {
        }
        public ChatServerMessage(sbyte channel, string content, int timestamp, string fingerprint, int senderId, string senderName) : base(channel, content, timestamp, fingerprint)
        {
            this.senderId = senderId;
            this.senderName = senderName;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(senderId);
            writer.WriteString(senderName);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            senderId = reader.ReadInt();
            senderName = reader.ReadString();
		}
	}
}
