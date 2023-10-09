using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record ExchangeKamaModifiedMessage : ExchangeObjectMessage
	{
        public int quantity;

        public new const int Id = 5521;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeKamaModifiedMessage()
        {
        }
        public ExchangeKamaModifiedMessage(bool remote, int quantity) : base(remote)
        {
            this.quantity = quantity;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(quantity);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            quantity = reader.ReadInt();
            if (quantity < 0)
                throw new Exception("Forbidden value on quantity = " + quantity + ", it doesn't respect the following condition : quantity < 0");
		}
	}
}
