using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record HouseSellFromInsideRequestMessage : HouseSellRequestMessage
	{
        public new const int Id = 5884;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public HouseSellFromInsideRequestMessage()
        {
        }
        public HouseSellFromInsideRequestMessage(int amount) : base(amount)
        {
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
		}
	}
}
