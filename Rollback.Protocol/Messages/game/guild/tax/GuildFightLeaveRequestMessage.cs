using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record GuildFightLeaveRequestMessage : Message
	{
        public int taxCollectorId;
        public int characterId;

        public const int Id = 5715;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GuildFightLeaveRequestMessage()
        {
        }
        public GuildFightLeaveRequestMessage(int taxCollectorId, int characterId)
        {
            this.taxCollectorId = taxCollectorId;
            this.characterId = characterId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(taxCollectorId);
            writer.WriteInt(characterId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            taxCollectorId = reader.ReadInt();
            characterId = reader.ReadInt();
            if (characterId < 0)
                throw new Exception("Forbidden value on characterId = " + characterId + ", it doesn't respect the following condition : characterId < 0");
		}
	}
}
