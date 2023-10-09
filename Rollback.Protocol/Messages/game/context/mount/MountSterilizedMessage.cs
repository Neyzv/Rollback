using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record MountSterilizedMessage : Message
	{
        public double mountId;

        public const int Id = 5977;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public MountSterilizedMessage()
        {
        }
        public MountSterilizedMessage(double mountId)
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
