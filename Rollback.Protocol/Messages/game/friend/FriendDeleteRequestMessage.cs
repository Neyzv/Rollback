using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record FriendDeleteRequestMessage : Message
	{
        public string name;

        public const int Id = 5603;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public FriendDeleteRequestMessage()
        {
        }
        public FriendDeleteRequestMessage(string name)
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
