using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record ExchangeErrorMessage : Message
	{
        public sbyte errorType;

        public const int Id = 5513;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeErrorMessage()
        {
        }
        public ExchangeErrorMessage(sbyte errorType)
        {
            this.errorType = errorType;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(errorType);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            errorType = reader.ReadSByte();
		}
	}
}
