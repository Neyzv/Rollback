using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record ExchangeReadyMessage : Message
	{
        public bool ready;

        public const int Id = 5511;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeReadyMessage()
        {
        }
        public ExchangeReadyMessage(bool ready)
        {
            this.ready = ready;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteBoolean(ready);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            ready = reader.ReadBoolean();
		}
	}
}
