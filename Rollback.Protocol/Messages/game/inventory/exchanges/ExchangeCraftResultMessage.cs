using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record ExchangeCraftResultMessage : Message
	{
        public sbyte craftResult;

        public const int Id = 5790;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeCraftResultMessage()
        {
        }
        public ExchangeCraftResultMessage(sbyte craftResult)
        {
            this.craftResult = craftResult;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(craftResult);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            craftResult = reader.ReadSByte();
            if (craftResult < 0)
                throw new Exception("Forbidden value on craftResult = " + craftResult + ", it doesn't respect the following condition : craftResult < 0");
		}
	}
}
