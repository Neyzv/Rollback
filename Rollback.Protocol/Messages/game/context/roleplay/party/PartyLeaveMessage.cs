using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record PartyLeaveMessage : Message
	{
        public const int Id = 5594;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public PartyLeaveMessage()
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
