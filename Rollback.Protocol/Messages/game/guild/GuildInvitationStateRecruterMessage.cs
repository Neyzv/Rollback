using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record GuildInvitationStateRecruterMessage : Message
	{
        public string recrutedName;
        public sbyte invitationState;

        public const int Id = 5563;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GuildInvitationStateRecruterMessage()
        {
        }
        public GuildInvitationStateRecruterMessage(string recrutedName, sbyte invitationState)
        {
            this.recrutedName = recrutedName;
            this.invitationState = invitationState;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteString(recrutedName);
            writer.WriteSByte(invitationState);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            recrutedName = reader.ReadString();
            invitationState = reader.ReadSByte();
            if (invitationState < 0)
                throw new Exception("Forbidden value on invitationState = " + invitationState + ", it doesn't respect the following condition : invitationState < 0");
		}
	}
}
