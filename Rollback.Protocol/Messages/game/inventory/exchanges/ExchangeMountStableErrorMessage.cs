using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record ExchangeMountStableErrorMessage : Message
	{
        public const int Id = 5981;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeMountStableErrorMessage()
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
