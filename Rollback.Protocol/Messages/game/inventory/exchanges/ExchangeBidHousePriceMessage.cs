using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record ExchangeBidHousePriceMessage : Message
	{
        public int genId;

        public const int Id = 5805;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeBidHousePriceMessage()
        {
        }
        public ExchangeBidHousePriceMessage(int genId)
        {
            this.genId = genId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(genId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            genId = reader.ReadInt();
            if (genId < 0)
                throw new Exception("Forbidden value on genId = " + genId + ", it doesn't respect the following condition : genId < 0");
		}
	}
}
