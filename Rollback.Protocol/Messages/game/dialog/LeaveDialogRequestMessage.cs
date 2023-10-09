using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record LeaveDialogRequestMessage : Message
	{
        public const int Id = 5501;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public LeaveDialogRequestMessage()
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
