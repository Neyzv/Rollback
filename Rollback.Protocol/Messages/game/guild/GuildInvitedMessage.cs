using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record GuildInvitedMessage : Message
	{
        public int recruterId;
        public string recruterName;
        public string guildName;

        public const int Id = 5552;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GuildInvitedMessage()
        {
        }
        public GuildInvitedMessage(int recruterId, string recruterName, string guildName)
        {
            this.recruterId = recruterId;
            this.recruterName = recruterName;
            this.guildName = guildName;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(recruterId);
            writer.WriteString(recruterName);
            writer.WriteString(guildName);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            recruterId = reader.ReadInt();
            if (recruterId < 0)
                throw new Exception("Forbidden value on recruterId = " + recruterId + ", it doesn't respect the following condition : recruterId < 0");
            recruterName = reader.ReadString();
            guildName = reader.ReadString();
		}
	}
}
