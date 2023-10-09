using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record MountRenamedMessage : Message
	{
        public double mountId;
        public string name;

        public const int Id = 5983;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public MountRenamedMessage()
        {
        }
        public MountRenamedMessage(double mountId, string name)
        {
            this.mountId = mountId;
            this.name = name;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteDouble(mountId);
            writer.WriteString(name);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            mountId = reader.ReadDouble();
            name = reader.ReadString();
		}
	}
}
