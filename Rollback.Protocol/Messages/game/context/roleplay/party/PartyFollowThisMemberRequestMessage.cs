using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record PartyFollowThisMemberRequestMessage : PartyFollowMemberRequestMessage
	{
        public bool enabled;

        public new const int Id = 5588;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public PartyFollowThisMemberRequestMessage()
        {
        }
        public PartyFollowThisMemberRequestMessage(int playerId, bool enabled) : base(playerId)
        {
            this.enabled = enabled;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteBoolean(enabled);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            enabled = reader.ReadBoolean();
		}
	}
}
