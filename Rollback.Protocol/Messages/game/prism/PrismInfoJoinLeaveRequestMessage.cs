using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record PrismInfoJoinLeaveRequestMessage : Message
	{
        public bool join;

        public const int Id = 5844;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public PrismInfoJoinLeaveRequestMessage()
        {
        }
        public PrismInfoJoinLeaveRequestMessage(bool join)
        {
            this.join = join;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteBoolean(join);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            join = reader.ReadBoolean();
		}
	}
}
