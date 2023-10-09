using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record ExchangeRequestOnTaxCollectorMessage : Message
	{
        public int taxCollectorId;

        public const int Id = 5779;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeRequestOnTaxCollectorMessage()
        {
        }
        public ExchangeRequestOnTaxCollectorMessage(int taxCollectorId)
        {
            this.taxCollectorId = taxCollectorId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(taxCollectorId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            taxCollectorId = reader.ReadInt();
		}
	}
}
