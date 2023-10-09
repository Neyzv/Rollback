using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record HouseLockFromInsideRequestMessage : LockableChangeCodeMessage
	{
        public new const int Id = 5885;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public HouseLockFromInsideRequestMessage()
        {
        }
        public HouseLockFromInsideRequestMessage(string code) : base(code)
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
