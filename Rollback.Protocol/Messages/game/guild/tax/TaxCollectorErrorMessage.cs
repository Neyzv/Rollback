using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record TaxCollectorErrorMessage : Message
	{
        public sbyte reason;

        public const int Id = 5634;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public TaxCollectorErrorMessage()
        {
        }
        public TaxCollectorErrorMessage(sbyte reason)
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
