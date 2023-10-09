using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record PartyUpdateMessage : Message
	{
        public PartyMemberInformations memberInformations;

        public const int Id = 5575;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public PartyUpdateMessage()
        {
        }
        public PartyUpdateMessage(PartyMemberInformations memberInformations)
        {
            this.memberInformations = memberInformations;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            memberInformations.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            memberInformations = new PartyMemberInformations();
            memberInformations.Deserialize(reader);
		}
	}
}
