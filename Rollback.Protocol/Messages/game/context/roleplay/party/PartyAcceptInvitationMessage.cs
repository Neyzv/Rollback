using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record PartyAcceptInvitationMessage : Message
	{
        public const int Id = 5580;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public PartyAcceptInvitationMessage()
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
