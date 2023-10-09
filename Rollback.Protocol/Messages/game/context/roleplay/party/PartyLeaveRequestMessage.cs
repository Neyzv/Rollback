using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record PartyLeaveRequestMessage : Message
	{
        public const int Id = 5593;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public PartyLeaveRequestMessage()
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
