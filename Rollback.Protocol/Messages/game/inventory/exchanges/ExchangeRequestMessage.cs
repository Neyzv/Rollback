using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record ExchangeRequestMessage : Message
	{
        public sbyte exchangeType;

        public const int Id = 5505;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeRequestMessage()
        {
        }
        public ExchangeRequestMessage(sbyte exchangeType)
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
