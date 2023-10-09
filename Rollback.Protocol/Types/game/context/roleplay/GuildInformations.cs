using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Types
{
    public record GuildInformations
    {
        public string guildName;
        public GuildEmblem guildEmblem;
        public const short Id = 127;
        public virtual short TypeId
        {
            get { return Id; }
        }
        public GuildInformations()
        {
        }
        public GuildInformations(string guildName, GuildEmblem guildEmblem)
        {
            this.guildName = guildName;
            this.guildEmblem = guildEmblem;
        }
        public virtual void Serialize(BigEndianWriter writer)
        {
            writer.WriteString(guildName);
            guildEmblem.Serialize(writer);
        }
        public virtual void Deserialize(BigEndianReader reader)
        {
            guildName = reader.ReadString();
            guildEmblem = new Types.GuildEmblem();
            guildEmblem.Deserialize(reader);
        }
    }
}
