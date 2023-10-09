using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record ChatAbstractClientMessage : Message
	{
        public string content;

        public const int Id = 850;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ChatAbstractClientMessage()
        {
        }
        public ChatAbstractClientMessage(string content)
        {
            this.content = content;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteString(content);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            content = reader.ReadString();
		}
	}
}
