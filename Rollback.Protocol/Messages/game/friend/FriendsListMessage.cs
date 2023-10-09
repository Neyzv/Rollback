using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record FriendsListMessage : Message
    {
        public FriendInformations[] friendsList;

        public const int Id = 4002;
        public override uint MessageId
        {
            get { return Id; }
        }
        public FriendsListMessage()
        {
        }
        public FriendsListMessage(FriendInformations[] friendsList)
        {
            this.friendsList = friendsList;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)friendsList.Length);
            foreach (var entry in friendsList)
            {
                writer.WriteShort(entry.TypeId);
                entry.Serialize(writer);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            friendsList = new FriendInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                friendsList[i] = (FriendInformations)ProtocolManager.Instance.GetType(reader.ReadUShort());
                friendsList[i].Deserialize(reader);
            }
        }
    }
}
