using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record ExchangeReplayStopMessage : Message
	{
        public const int Id = 6001;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeReplayStopMessage()
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
