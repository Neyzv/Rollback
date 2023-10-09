using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record ExchangeShowVendorTaxMessage : Message
	{
        public const int Id = 5783;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeShowVendorTaxMessage()
        {
        }
        public override void Serialize(BigEndianWriter writer)
        {
        }
        public override void Deserialize(BigEndianReader reader)
        {
		}
	}
}
