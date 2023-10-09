using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record HouseBuyRequestMessage : Message
	{
        public int proposedPrice;

        public const int Id = 5738;
        public override uint MessageId
        {
        	get { return 5738; }
        }
        public HouseBuyRequestMessage()
        {
        }
        public HouseBuyRequestMessage(int proposedPrice)
        {
            this.proposedPrice = proposedPrice;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(proposedPrice);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            proposedPrice = reader.ReadInt();
            if (proposedPrice < 0)
                throw new Exception("Forbidden value on proposedPrice = " + proposedPrice + ", it doesn't respect the following condition : proposedPrice < 0");
		}
	}
}
