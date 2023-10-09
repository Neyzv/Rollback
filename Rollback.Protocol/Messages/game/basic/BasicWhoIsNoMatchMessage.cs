using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record BasicWhoIsNoMatchMessage : Message
	{
        public string search;

        public const int Id = 179;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public BasicWhoIsNoMatchMessage()
        {
        }
        public BasicWhoIsNoMatchMessage(string search)
        {
            this.search = search;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteString(search);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            search = reader.ReadString();
		}
	}
}
