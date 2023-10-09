using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record HouseKickIndoorMerchantRequestMessage : Message
	{
        public short cellId;

        public const int Id = 5661;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public HouseKickIndoorMerchantRequestMessage()
        {
        }
        public HouseKickIndoorMerchantRequestMessage(short cellId)
        {
            this.cellId = cellId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(cellId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            cellId = reader.ReadShort();
            if (cellId < 0 || cellId > 559)
                throw new Exception("Forbidden value on cellId = " + cellId + ", it doesn't respect the following condition : cellId < 0 || cellId > 559");
		}
	}
}
