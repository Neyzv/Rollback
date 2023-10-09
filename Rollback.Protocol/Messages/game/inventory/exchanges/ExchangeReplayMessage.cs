using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record ExchangeReplayMessage : Message
	{
        public int count;

        public const int Id = 6002;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeReplayMessage()
        {
        }
        public ExchangeReplayMessage(int count)
        {
            this.count = count;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(count);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            count = reader.ReadInt();
		}
	}
}
