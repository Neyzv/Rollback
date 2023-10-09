using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record GuildMemberOnlineStatusMessage : Message
	{
        public int memberId;
        public bool online;

        public const int Id = 6061;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GuildMemberOnlineStatusMessage()
        {
        }
        public GuildMemberOnlineStatusMessage(int memberId, bool online)
        {
            this.memberId = memberId;
            this.online = online;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(memberId);
            writer.WriteBoolean(online);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            memberId = reader.ReadInt();
            if (memberId < 0)
                throw new Exception("Forbidden value on memberId = " + memberId + ", it doesn't respect the following condition : memberId < 0");
            online = reader.ReadBoolean();
		}
	}
}
