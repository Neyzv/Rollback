using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record GuildCreationValidMessage : Message
	{
        public string guildName;
        public GuildEmblem guildEmblem;

        public const int Id = 5546;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GuildCreationValidMessage()
        {
        }
        public GuildCreationValidMessage(string guildName, GuildEmblem guildEmblem)
        {
            this.guildName = guildName;
            this.guildEmblem = guildEmblem;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteString(guildName);
            guildEmblem.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            guildName = reader.ReadString();
            guildEmblem = new GuildEmblem();
            guildEmblem.Deserialize(reader);
		}
	}
}
