using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record ExchangeRequestedTradeMessage : ExchangeRequestedMessage
	{
        public int source;
        public int target;

        public new const int Id = 5523;
        public override uint MessageId
        {
        	get { return 5523; }
        }
        public ExchangeRequestedTradeMessage()
        {
        }
        public ExchangeRequestedTradeMessage(sbyte exchangeType, int source, int target) : base(exchangeType)
        {
            this.source = source;
            this.target = target;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(source);
            writer.WriteInt(target);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            source = reader.ReadInt();
            if (source < 0)
                throw new Exception("Forbidden value on source = " + source + ", it doesn't respect the following condition : source < 0");
            target = reader.ReadInt();
            if (target < 0)
                throw new Exception("Forbidden value on target = " + target + ", it doesn't respect the following condition : target < 0");
		}
	}
}
