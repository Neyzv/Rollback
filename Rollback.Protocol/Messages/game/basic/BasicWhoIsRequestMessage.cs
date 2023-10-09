using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record BasicWhoIsRequestMessage : Message
	{
        public string search;

        public const int Id = 181;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public BasicWhoIsRequestMessage()
        {
        }
        public BasicWhoIsRequestMessage(string search)
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
