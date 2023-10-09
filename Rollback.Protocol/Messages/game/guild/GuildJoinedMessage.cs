using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;


namespace Rollback.Protocol.Messages
{
	public record GuildJoinedMessage : Message
	{
        public string guildName;
        public GuildEmblem guildEmblem;
        public uint memberRights;

        public const int Id = 5564;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GuildJoinedMessage()
        {
        }
        public GuildJoinedMessage(string guildName, GuildEmblem guildEmblem, uint memberRights)
        {
            this.guildName = guildName;
            this.guildEmblem = guildEmblem;
            this.memberRights = memberRights;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteString(guildName);
            guildEmblem.Serialize(writer);
            writer.WriteUInt(memberRights);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            guildName = reader.ReadString();
            guildEmblem = new GuildEmblem();
            guildEmblem.Deserialize(reader);
            memberRights = reader.ReadUInt();
            if (memberRights < 0 || memberRights > 4294967295)
                throw new Exception("Forbidden value on memberRights = " + memberRights + ", it doesn't respect the following condition : memberRights < 0 || memberRights > 4294967295");
		}
	}
}
