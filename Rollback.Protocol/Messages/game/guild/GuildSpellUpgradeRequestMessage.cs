using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record GuildSpellUpgradeRequestMessage : Message
	{
        public int spellId;

        public const int Id = 5699;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GuildSpellUpgradeRequestMessage()
        {
        }
        public GuildSpellUpgradeRequestMessage(int spellId)
        {
            this.spellId = spellId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(spellId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            spellId = reader.ReadInt();
            if (spellId < 0)
                throw new Exception("Forbidden value on spellId = " + spellId + ", it doesn't respect the following condition : spellId < 0");
		}
	}
}
