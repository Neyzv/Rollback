using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record ExchangeReplayCountModifiedMessage : Message
	{
        public int count;

        public const int Id = 6023;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeReplayCountModifiedMessage()
        {
        }
        public ExchangeReplayCountModifiedMessage(int count)
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
