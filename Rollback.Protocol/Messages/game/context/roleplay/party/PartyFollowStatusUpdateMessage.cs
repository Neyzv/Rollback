using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record PartyFollowStatusUpdateMessage : Message
	{
        public bool success;
        public int followedId;

        public const int Id = 5581;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public PartyFollowStatusUpdateMessage()
        {
        }
        public PartyFollowStatusUpdateMessage(bool success, int followedId)
        {
            this.success = success;
            this.followedId = followedId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteBoolean(success);
            writer.WriteInt(followedId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            success = reader.ReadBoolean();
            followedId = reader.ReadInt();
            if (followedId < 0)
                throw new Exception("Forbidden value on followedId = " + followedId + ", it doesn't respect the following condition : followedId < 0");
		}
	}
}
