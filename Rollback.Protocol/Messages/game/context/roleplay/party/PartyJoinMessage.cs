using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;


namespace Rollback.Protocol.Messages
{
	public record PartyJoinMessage : Message
	{
        public int partyLeaderId;
        public PartyMemberInformations[] members;

        public const int Id = 5576;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public PartyJoinMessage()
        {
        }
        public PartyJoinMessage(int partyLeaderId, PartyMemberInformations[] members)
        {
            this.partyLeaderId = partyLeaderId;
            this.members = members;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(partyLeaderId);
            writer.WriteUShort((ushort)members.Length);
            foreach (var entry in members)
            {
                 entry.Serialize(writer);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            partyLeaderId = reader.ReadInt();
            if (partyLeaderId < 0)
                throw new Exception("Forbidden value on partyLeaderId = " + partyLeaderId + ", it doesn't respect the following condition : partyLeaderId < 0");
            var limit = reader.ReadUShort();
            members = new PartyMemberInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 members[i] = new PartyMemberInformations();
                 members[i].Deserialize(reader);
            }
		}
	}
}
