using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record LeaveDialogMessage : Message
	{
        public const int Id = 5502;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public LeaveDialogMessage()
        {
        }
        public override void Serialize(BigEndianWriter writer)
        {
        }
        public override void Deserialize(BigEndianReader reader)
        {
		}
	}
}
