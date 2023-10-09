using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record FriendDeleteResultMessage : Message
	{
        public bool success;
        public string name;

        public const int Id = 5601;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public FriendDeleteResultMessage()
        {
        }
        public FriendDeleteResultMessage(bool success, string name)
        {
            this.success = success;
            this.name = name;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteBoolean(success);
            writer.WriteString(name);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            success = reader.ReadBoolean();
            name = reader.ReadString();
		}
	}
}
