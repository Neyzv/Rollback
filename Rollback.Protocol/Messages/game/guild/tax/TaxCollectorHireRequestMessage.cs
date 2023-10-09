using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record TaxCollectorHireRequestMessage : Message
	{
        public const int Id = 5681;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public TaxCollectorHireRequestMessage()
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
