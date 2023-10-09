using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record HouseSellRequestMessage : Message
	{
        public int amount;

        public const int Id = 5697;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public HouseSellRequestMessage()
        {
        }
        public HouseSellRequestMessage(int amount)
        {
            this.amount = amount;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(amount);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            amount = reader.ReadInt();
            if (amount < 0)
                throw new Exception("Forbidden value on amount = " + amount + ", it doesn't respect the following condition : amount < 0");
		}
	}
}
