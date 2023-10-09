using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record MountRenameRequestMessage : Message
	{
        public string name;
        public double mountId;

        public const int Id = 5987;
        public override uint MessageId
        {
        	get { return 5987; }
        }
        public MountRenameRequestMessage()
        {
        }
        public MountRenameRequestMessage(string name, double mountId)
        {
            this.name = name;
            this.mountId = mountId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteString(name);
            writer.WriteDouble(mountId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            name = reader.ReadString();
            mountId = reader.ReadDouble();
		}
	}
}
