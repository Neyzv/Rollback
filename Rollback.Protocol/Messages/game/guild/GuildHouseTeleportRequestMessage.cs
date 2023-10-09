using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record GuildHouseTeleportRequestMessage : Message
	{
        public int houseId;

        public const int Id = 5712;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GuildHouseTeleportRequestMessage()
        {
        }
        public GuildHouseTeleportRequestMessage(int houseId)
        {
            this.houseId = houseId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(houseId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            houseId = reader.ReadInt();
            if (houseId < 0)
                throw new Exception("Forbidden value on houseId = " + houseId + ", it doesn't respect the following condition : houseId < 0");
		}
	}
}
