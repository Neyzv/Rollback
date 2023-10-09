using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record HelloConnectMessage : Message
	{
        public string key;

        public const int Id = 3;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public HelloConnectMessage()
        {
        }
        public HelloConnectMessage(string key)
        {
            this.key = key;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteString(key);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            key = reader.ReadString();
		}
	}
}
