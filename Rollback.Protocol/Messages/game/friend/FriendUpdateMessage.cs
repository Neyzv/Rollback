using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record FriendUpdateMessage : Message
    {
        public FriendInformations friendUpdated;

        public const int Id = 5924;
        public override uint MessageId
        {
            get { return Id; }
        }
        public FriendUpdateMessage()
        {
        }
        public FriendUpdateMessage(FriendInformations friendUpdated)
        {
            this.friendUpdated = friendUpdated;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(friendUpdated.TypeId);
            friendUpdated.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            friendUpdated = (FriendInformations)ProtocolManager.Instance.GetType(reader.ReadUShort());
            friendUpdated.Deserialize(reader);
        }
    }
}
