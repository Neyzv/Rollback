using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record HouseGuildNoneMessage : Message
	{
        public short houseId;

        public const int Id = 5701;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public HouseGuildNoneMessage()
        {
        }
        public HouseGuildNoneMessage(short houseId)
        {
            this.houseId = houseId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(houseId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            houseId = reader.ReadShort();
            if (houseId < 0)
                throw new Exception("Forbidden value on houseId = " + houseId + ", it doesn't respect the following condition : houseId < 0");
		}
	}
}
