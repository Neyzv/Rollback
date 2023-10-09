using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record ExchangeStartAsVendorMessage : Message
	{
        public const int Id = 5775;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeStartAsVendorMessage()
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
