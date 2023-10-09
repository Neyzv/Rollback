using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record SpellUpgradeSuccessMessage : Message
	{
        public int spellId;
        public sbyte spellLevel;

        public const int Id = 1201;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public SpellUpgradeSuccessMessage()
        {
        }
        public SpellUpgradeSuccessMessage(int spellId, sbyte spellLevel)
        {
            this.spellId = spellId;
            this.spellLevel = spellLevel;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(spellId);
            writer.WriteSByte(spellLevel);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            spellId = reader.ReadInt();
            spellLevel = reader.ReadSByte();
            if (spellLevel < 1 || spellLevel > 6)
                throw new Exception("Forbidden value on spellLevel = " + spellLevel + ", it doesn't respect the following condition : spellLevel < 1 || spellLevel > 6");
		}
	}
}
