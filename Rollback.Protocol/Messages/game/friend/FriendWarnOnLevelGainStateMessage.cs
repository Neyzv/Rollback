using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record FriendWarnOnLevelGainStateMessage : Message
	{
        public bool enable;

        public const int Id = 6078;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public FriendWarnOnLevelGainStateMessage()
        {
        }
        public FriendWarnOnLevelGainStateMessage(bool enable)
        {
            this.enable = enable;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteBoolean(enable);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            enable = reader.ReadBoolean();
		}
	}
}
