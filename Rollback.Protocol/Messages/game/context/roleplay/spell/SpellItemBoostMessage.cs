using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record SpellItemBoostMessage : Message
	{
        public int statId;
        public short spellId;
        public short value;

        public const int Id = 6011;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public SpellItemBoostMessage()
        {
        }
        public SpellItemBoostMessage(int statId, short spellId, short value)
        {
            this.statId = statId;
            this.spellId = spellId;
            this.value = value;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(statId);
            writer.WriteShort(spellId);
            writer.WriteShort(value);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            statId = reader.ReadInt();
            if (statId < 0)
                throw new Exception("Forbidden value on statId = " + statId + ", it doesn't respect the following condition : statId < 0");
            spellId = reader.ReadShort();
            if (spellId < 0)
                throw new Exception("Forbidden value on spellId = " + spellId + ", it doesn't respect the following condition : spellId < 0");
            value = reader.ReadShort();
		}
	}
}
