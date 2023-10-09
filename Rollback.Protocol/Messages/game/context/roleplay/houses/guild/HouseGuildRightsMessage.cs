using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;


namespace Rollback.Protocol.Messages
{
	public record HouseGuildRightsMessage : Message
	{
        public short houseId;
        public string guildName;
        public GuildEmblem guildEmblem;
        public uint rights;

        public const int Id = 5703;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public HouseGuildRightsMessage()
        {
        }
        public HouseGuildRightsMessage(short houseId, string guildName, GuildEmblem guildEmblem, uint rights)
        {
            this.houseId = houseId;
            this.guildName = guildName;
            this.guildEmblem = guildEmblem;
            this.rights = rights;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(houseId);
            writer.WriteString(guildName);
            guildEmblem.Serialize(writer);
            writer.WriteUInt(rights);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            houseId = reader.ReadShort();
            if (houseId < 0)
                throw new Exception("Forbidden value on houseId = " + houseId + ", it doesn't respect the following condition : houseId < 0");
            guildName = reader.ReadString();
            guildEmblem = new GuildEmblem();
            guildEmblem.Deserialize(reader);
            rights = reader.ReadUInt();
            if (rights < 0 || rights > 4294967295)
                throw new Exception("Forbidden value on rights = " + rights + ", it doesn't respect the following condition : rights < 0 || rights > 4294967295");
		}
	}
}
