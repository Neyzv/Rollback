using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record HouseGuildRightsChangeRequestMessage : Message
	{
        public uint rights;

        public const int Id = 5702;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public HouseGuildRightsChangeRequestMessage()
        {
        }
        public HouseGuildRightsChangeRequestMessage(uint rights)
        {
            this.rights = rights;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUInt(rights);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            rights = reader.ReadUInt();
            if (rights < 0 || rights > 4294967295)
                throw new Exception("Forbidden value on rights = " + rights + ", it doesn't respect the following condition : rights < 0 || rights > 4294967295");
		}
	}
}
