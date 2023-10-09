using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Types
{
    public record PaddockPrivateInformations : PaddockAbandonnedInformations
    {
        public string guildName;
        public GuildEmblem guildEmblem;
        public new const short Id = 131;
        public override short TypeId
        {
            get { return Id; }
        }
        public PaddockPrivateInformations()
        {
        }
        public PaddockPrivateInformations(short maxOutdoorMount, short maxItems, int price, int guildId, string guildName, GuildEmblem guildEmblem) : base(maxOutdoorMount, maxItems, price, guildId)
        {
            this.guildName = guildName;
            this.guildEmblem = guildEmblem;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteString(guildName);
            guildEmblem.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            guildName = reader.ReadString();
            guildEmblem = new GuildEmblem();
            guildEmblem.Deserialize(reader);
        }
    }
}