using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record ChatClientPrivateMessage : ChatAbstractClientMessage
	{
        public string receiver;

        public new const int Id = 851;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ChatClientPrivateMessage()
        {
        }
        public ChatClientPrivateMessage(string content, string receiver) : base(content)
        {
            this.receiver = receiver;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteString(receiver);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            receiver = reader.ReadString();
		}
	}
}
