using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record ClientKeyMessage : Message
	{
        public string key;

        public const int Id = 5607;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ClientKeyMessage()
        {
        }
        public ClientKeyMessage(string key)
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
