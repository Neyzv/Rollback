using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record PartyLeaderUpdateMessage : Message
	{
        public int partyLeaderId;

        public const int Id = 5578;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public PartyLeaderUpdateMessage()
        {
        }
        public PartyLeaderUpdateMessage(int partyLeaderId)
        {
            this.partyLeaderId = partyLeaderId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(partyLeaderId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            partyLeaderId = reader.ReadInt();
            if (partyLeaderId < 0)
                throw new Exception("Forbidden value on partyLeaderId = " + partyLeaderId + ", it doesn't respect the following condition : partyLeaderId < 0");
		}
	}
}
