using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record PartyAbdicateThroneMessage : Message
	{
        public int playerId;

        public const int Id = 6080;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public PartyAbdicateThroneMessage()
        {
        }
        public PartyAbdicateThroneMessage(int playerId)
        {
            this.playerId = playerId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(playerId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            playerId = reader.ReadInt();
            if (playerId < 0)
                throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0");
		}
	}
}
