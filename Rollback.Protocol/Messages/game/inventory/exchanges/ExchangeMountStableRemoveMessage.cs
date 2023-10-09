using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record ExchangeMountStableRemoveMessage : Message
	{
        public double mountId;

        public const int Id = 5964;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeMountStableRemoveMessage()
        {
        }
        public ExchangeMountStableRemoveMessage(double mountId)
        {
            this.mountId = mountId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteDouble(mountId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            mountId = reader.ReadDouble();
		}
	}
}
