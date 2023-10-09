using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Types
{
    public record HouseInformationsExtended : HouseInformations
    {
        public string guildName;
        public GuildEmblem guildEmblem;
        public new const short Id = 112;
        public override short TypeId
        {
            get { return Id; }
        }
        public HouseInformationsExtended()
        {
        }
        public HouseInformationsExtended(int houseId, int[] doorsOnMap, string ownerName, bool isOnSale, short modelId, string guildName, GuildEmblem guildEmblem) : base(houseId, doorsOnMap, ownerName, isOnSale, modelId)
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
            guildEmblem = new Types.GuildEmblem();
            guildEmblem.Deserialize(reader);
        }
    }
}
