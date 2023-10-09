using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record FriendsListWithSpouseMessage : FriendsListMessage
    {
        public FriendSpouseInformations spouse;

        public new const int Id = 5931;
        public override uint MessageId
        {
            get { return Id; }
        }
        public FriendsListWithSpouseMessage()
        {
        }
        public FriendsListWithSpouseMessage(FriendInformations[] friendsList, FriendSpouseInformations spouse) : base(friendsList)
        {
            this.spouse = spouse;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(spouse.TypeId);
            spouse.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            spouse = (FriendSpouseInformations)ProtocolManager.Instance.GetType(reader.ReadUShort());
            spouse.Deserialize(reader);
        }
    }
}
