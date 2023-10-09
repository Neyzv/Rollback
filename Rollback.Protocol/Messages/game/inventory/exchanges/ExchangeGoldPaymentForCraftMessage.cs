using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record ExchangeGoldPaymentForCraftMessage : Message
	{
        public bool onlySuccess;
        public int goldSum;

        public const int Id = 5833;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeGoldPaymentForCraftMessage()
        {
        }
        public ExchangeGoldPaymentForCraftMessage(bool onlySuccess, int goldSum)
        {
            this.onlySuccess = onlySuccess;
            this.goldSum = goldSum;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteBoolean(onlySuccess);
            writer.WriteInt(goldSum);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            onlySuccess = reader.ReadBoolean();
            goldSum = reader.ReadInt();
            if (goldSum < 0)
                throw new Exception("Forbidden value on goldSum = " + goldSum + ", it doesn't respect the following condition : goldSum < 0");
		}
	}
}
