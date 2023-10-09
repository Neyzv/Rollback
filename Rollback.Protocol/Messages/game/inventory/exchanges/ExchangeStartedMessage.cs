using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record ExchangeStartedMessage : Message
	{
        public sbyte exchangeType;

        public const int Id = 5512;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeStartedMessage()
        {
        }
        public ExchangeStartedMessage(sbyte exchangeType)
        {
            this.exchangeType = exchangeType;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(exchangeType);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            exchangeType = reader.ReadSByte();
		}
	}
}
