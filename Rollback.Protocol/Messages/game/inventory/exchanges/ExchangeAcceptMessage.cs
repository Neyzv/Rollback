using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record ExchangeAcceptMessage : Message
	{
        public const int Id = 5508;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeAcceptMessage()
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
