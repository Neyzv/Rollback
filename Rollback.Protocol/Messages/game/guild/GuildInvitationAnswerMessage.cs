using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record GuildInvitationAnswerMessage : Message
	{
        public bool accept;

        public const int Id = 5556;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GuildInvitationAnswerMessage()
        {
        }
        public GuildInvitationAnswerMessage(bool accept)
        {
            this.accept = accept;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteBoolean(accept);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            accept = reader.ReadBoolean();
		}
	}
}
