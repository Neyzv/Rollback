using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record GuildMemberLeavingMessage : Message
	{
        public bool kicked;
        public int memberId;

        public const int Id = 5923;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GuildMemberLeavingMessage()
        {
        }
        public GuildMemberLeavingMessage(bool kicked, int memberId)
        {
            this.kicked = kicked;
            this.memberId = memberId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteBoolean(kicked);
            writer.WriteInt(memberId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            kicked = reader.ReadBoolean();
            memberId = reader.ReadInt();
		}
	}
}
