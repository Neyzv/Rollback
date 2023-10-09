using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record IgnoredDeleteRequestMessage : Message
	{
        public string name;

        public const int Id = 5680;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public IgnoredDeleteRequestMessage()
        {
        }
        public IgnoredDeleteRequestMessage(string name)
        {
            this.name = name;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteString(name);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            name = reader.ReadString();
		}
	}
}
