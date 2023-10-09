using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record ExchangeObjectMessage : Message
	{
        public bool remote;

        public const int Id = 5515;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeObjectMessage()
        {
        }
        public ExchangeObjectMessage(bool remote)
        {
            this.remote = remote;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteBoolean(remote);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            remote = reader.ReadBoolean();
		}
	}
}
