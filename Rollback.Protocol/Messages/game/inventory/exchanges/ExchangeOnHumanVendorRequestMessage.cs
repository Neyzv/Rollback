using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record ExchangeOnHumanVendorRequestMessage : Message
	{
        public int humanVendorId;
        public int humanVendorCell;

        public const int Id = 5772;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeOnHumanVendorRequestMessage()
        {
        }
        public ExchangeOnHumanVendorRequestMessage(int humanVendorId, int humanVendorCell)
        {
            this.humanVendorId = humanVendorId;
            this.humanVendorCell = humanVendorCell;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(humanVendorId);
            writer.WriteInt(humanVendorCell);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            humanVendorId = reader.ReadInt();
            if (humanVendorId < 0)
                throw new Exception("Forbidden value on humanVendorId = " + humanVendorId + ", it doesn't respect the following condition : humanVendorId < 0");
            humanVendorCell = reader.ReadInt();
            if (humanVendorCell < 0)
                throw new Exception("Forbidden value on humanVendorCell = " + humanVendorCell + ", it doesn't respect the following condition : humanVendorCell < 0");
		}
	}
}
