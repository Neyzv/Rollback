using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record PartyInvitationMessage : Message
	{
        public int fromId;
        public string fromName;
        public int toId;
        public string toName;

        public const int Id = 5586;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public PartyInvitationMessage()
        {
        }
        public PartyInvitationMessage(int fromId, string fromName, int toId, string toName)
        {
            this.fromId = fromId;
            this.fromName = fromName;
            this.toId = toId;
            this.toName = toName;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(fromId);
            writer.WriteString(fromName);
            writer.WriteInt(toId);
            writer.WriteString(toName);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            fromId = reader.ReadInt();
            if (fromId < 0)
                throw new Exception("Forbidden value on fromId = " + fromId + ", it doesn't respect the following condition : fromId < 0");
            fromName = reader.ReadString();
            toId = reader.ReadInt();
            if (toId < 0)
                throw new Exception("Forbidden value on toId = " + toId + ", it doesn't respect the following condition : toId < 0");
            toName = reader.ReadString();
		}
	}
}
