using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record FriendAddedMessage : Message
    {
        public FriendInformations friendAdded;

        public const int Id = 5599;
        public override uint MessageId
        {
            get { return Id; }
        }
        public FriendAddedMessage()
        {
        }
        public FriendAddedMessage(FriendInformations friendAdded)
        {
            this.friendAdded = friendAdded;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(friendAdded.TypeId);
            friendAdded.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            friendAdded = (FriendInformations)ProtocolManager.Instance.GetType(reader.ReadUShort());
            friendAdded.Deserialize(reader);
        }
    }
}
