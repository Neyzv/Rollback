using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record FriendAddRequestMessage : Message
	{
        public string name;

        public const int Id = 4004;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public FriendAddRequestMessage()
        {
        }
        public FriendAddRequestMessage(string name)
        {
            this.name = name;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteString(name);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            name = reader.ReadString();
		}
	}
}
