using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record PartyRefuseInvitationMessage : Message
	{
        public const int Id = 5582;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public PartyRefuseInvitationMessage()
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
