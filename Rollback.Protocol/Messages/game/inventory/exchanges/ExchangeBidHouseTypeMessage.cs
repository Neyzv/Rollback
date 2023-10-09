using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record ExchangeBidHouseTypeMessage : Message
	{
        public int type;

        public const int Id = 5803;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeBidHouseTypeMessage()
        {
        }
        public ExchangeBidHouseTypeMessage(int type)
        {
            this.type = type;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(type);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            type = reader.ReadInt();
            if (type < 0)
                throw new Exception("Forbidden value on type = " + type + ", it doesn't respect the following condition : type < 0");
		}
	}
}
