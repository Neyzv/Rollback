using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record IgnoredAddRequestMessage : Message
	{
        public string name;

        public const int Id = 5673;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public IgnoredAddRequestMessage()
        {
        }
        public IgnoredAddRequestMessage(string name)
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
