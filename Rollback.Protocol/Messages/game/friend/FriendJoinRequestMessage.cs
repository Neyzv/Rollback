using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record FriendJoinRequestMessage : Message
	{
        public string name;

        public const int Id = 5605;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public FriendJoinRequestMessage()
        {
        }
        public FriendJoinRequestMessage(string name)
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
