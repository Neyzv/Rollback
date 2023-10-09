using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record ExchangeWaitingResultMessage : Message
	{
        public bool bwait;

        public const int Id = 5786;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeWaitingResultMessage()
        {
        }
        public ExchangeWaitingResultMessage(bool bwait)
        {
            this.bwait = bwait;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteBoolean(bwait);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            bwait = reader.ReadBoolean();
		}
	}
}
