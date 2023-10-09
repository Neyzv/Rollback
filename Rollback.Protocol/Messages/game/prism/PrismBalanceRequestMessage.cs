using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record PrismBalanceRequestMessage : Message
	{
        public const int Id = 5839;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public PrismBalanceRequestMessage()
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
