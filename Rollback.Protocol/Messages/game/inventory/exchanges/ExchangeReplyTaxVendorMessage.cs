using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record ExchangeReplyTaxVendorMessage : Message
	{
        public int objectValue;
        public int taxRate;
        public int totalTaxValue;

        public const int Id = 5787;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeReplyTaxVendorMessage()
        {
        }
        public ExchangeReplyTaxVendorMessage(int objectValue, int taxRate, int totalTaxValue)
        {
            this.objectValue = objectValue;
            this.taxRate = taxRate;
            this.totalTaxValue = totalTaxValue;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(objectValue);
            writer.WriteInt(taxRate);
            writer.WriteInt(totalTaxValue);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            objectValue = reader.ReadInt();
            if (objectValue < 0)
                throw new Exception("Forbidden value on objectValue = " + objectValue + ", it doesn't respect the following condition : objectValue < 0");
            taxRate = reader.ReadInt();
            if (taxRate < 0)
                throw new Exception("Forbidden value on taxRate = " + taxRate + ", it doesn't respect the following condition : taxRate < 0");
            totalTaxValue = reader.ReadInt();
            if (totalTaxValue < 0)
                throw new Exception("Forbidden value on totalTaxValue = " + totalTaxValue + ", it doesn't respect the following condition : totalTaxValue < 0");
		}
	}
}
