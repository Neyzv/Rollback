using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record ObjectErrorMessage : Message
	{
        public sbyte reason;

        public const int Id = 3004;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ObjectErrorMessage()
        {
        }
        public ObjectErrorMessage(sbyte reason)
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
