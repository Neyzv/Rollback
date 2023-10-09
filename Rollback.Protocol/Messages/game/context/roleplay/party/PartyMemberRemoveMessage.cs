using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record PartyMemberRemoveMessage : Message
	{
        public int leavingPlayerId;

        public const int Id = 5579;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public PartyMemberRemoveMessage()
        {
        }
        public PartyMemberRemoveMessage(int leavingPlayerId)
        {
            this.leavingPlayerId = leavingPlayerId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(leavingPlayerId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            leavingPlayerId = reader.ReadInt();
            if (leavingPlayerId < 0)
                throw new Exception("Forbidden value on leavingPlayerId = " + leavingPlayerId + ", it doesn't respect the following condition : leavingPlayerId < 0");
		}
	}
}
