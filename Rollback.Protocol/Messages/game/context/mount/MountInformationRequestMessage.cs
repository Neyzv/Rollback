using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record MountInformationRequestMessage : Message
	{
        public double id;
        public double time;

        public const int Id = 5972;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public MountInformationRequestMessage()
        {
        }
        public MountInformationRequestMessage(double id, double time)
        {
            this.id = id;
            this.time = time;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteDouble(id);
            writer.WriteDouble(time);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            id = reader.ReadDouble();
            time = reader.ReadDouble();
		}
	}
}
