using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record ChatServerCopyMessage : ChatAbstractServerMessage
	{
        public int receiverId;
        public string receiverName;

        public new const int Id = 882;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ChatServerCopyMessage()
        {
        }
        public ChatServerCopyMessage(sbyte channel, string content, int timestamp, string fingerprint, int receiverId, string receiverName) : base(channel, content, timestamp, fingerprint)
        {
            this.receiverId = receiverId;
            this.receiverName = receiverName;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(receiverId);
            writer.WriteString(receiverName);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            receiverId = reader.ReadInt();
            if (receiverId < 0)
                throw new Exception("Forbidden value on receiverId = " + receiverId + ", it doesn't respect the following condition : receiverId < 0");
            receiverName = reader.ReadString();
		}
	}
}
