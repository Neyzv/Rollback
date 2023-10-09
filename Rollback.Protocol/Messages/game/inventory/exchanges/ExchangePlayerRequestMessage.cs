using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record ExchangePlayerRequestMessage : ExchangeRequestMessage
	{
        public int target;

        public new const int Id = 5773;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangePlayerRequestMessage()
        {
        }
        public ExchangePlayerRequestMessage(sbyte exchangeType, int target) : base(exchangeType)
        {
            this.target = target;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(target);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            target = reader.ReadInt();
            if (target < 0)
                throw new Exception("Forbidden value on target = " + target + ", it doesn't respect the following condition : target < 0");
		}
	}
}
