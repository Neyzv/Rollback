using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record ExchangeObjectTransfertAllToInvMessage : Message
	{
        public const int Id = 6032;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeObjectTransfertAllToInvMessage()
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
