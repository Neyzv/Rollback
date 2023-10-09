using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record PrismUseRequestMessage : Message
	{
        public const int Id = 6041;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public PrismUseRequestMessage()
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
