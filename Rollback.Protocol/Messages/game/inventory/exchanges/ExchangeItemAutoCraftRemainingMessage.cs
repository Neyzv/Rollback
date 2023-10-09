using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record ExchangeItemAutoCraftRemainingMessage : Message
	{
        public int count;

        public const int Id = 6015;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeItemAutoCraftRemainingMessage()
        {
        }
        public ExchangeItemAutoCraftRemainingMessage(int count)
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
            if (count < 0)
                throw new Exception("Forbidden value on count = " + count + ", it doesn't respect the following condition : count < 0");
		}
	}
}
