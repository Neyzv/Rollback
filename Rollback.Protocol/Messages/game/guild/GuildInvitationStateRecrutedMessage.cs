using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record GuildInvitationStateRecrutedMessage : Message
	{
        public sbyte invitationState;

        public const int Id = 5548;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GuildInvitationStateRecrutedMessage()
        {
        }
        public GuildInvitationStateRecrutedMessage(sbyte invitationState)
        {
            this.invitationState = invitationState;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(invitationState);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            invitationState = reader.ReadSByte();
            if (invitationState < 0)
                throw new Exception("Forbidden value on invitationState = " + invitationState + ", it doesn't respect the following condition : invitationState < 0");
		}
	}
}
