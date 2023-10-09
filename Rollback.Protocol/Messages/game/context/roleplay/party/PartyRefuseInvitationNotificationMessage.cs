using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record PartyRefuseInvitationNotificationMessage : Message
	{
        public const int Id = 5596;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public PartyRefuseInvitationNotificationMessage()
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
