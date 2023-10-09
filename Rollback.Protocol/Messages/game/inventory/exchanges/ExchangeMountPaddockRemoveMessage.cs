using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record ExchangeMountPaddockRemoveMessage : Message
	{
        public double mountId;

        public const int Id = 6050;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeMountPaddockRemoveMessage()
        {
        }
        public ExchangeMountPaddockRemoveMessage(double mountId)
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
