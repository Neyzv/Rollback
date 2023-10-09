using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record ExchangeItemAutoCraftStopedMessage : Message
	{
        public sbyte reason;

        public const int Id = 5810;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeItemAutoCraftStopedMessage()
        {
        }
        public ExchangeItemAutoCraftStopedMessage(sbyte reason)
        {
            this.reason = reason;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(reason);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            reason = reader.ReadSByte();
		}
	}
}
