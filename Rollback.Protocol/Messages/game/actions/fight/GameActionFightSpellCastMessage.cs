using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record GameActionFightSpellCastMessage : AbstractGameActionFightTargetedAbilityMessage
	{
        public short spellId;
        public sbyte spellLevel;

        public new const int Id = 1010;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameActionFightSpellCastMessage()
        {
        }
        public GameActionFightSpellCastMessage(short actionId, int sourceId, short destinationCellId, sbyte critical, bool silentCast, short spellId, sbyte spellLevel) : base(actionId, sourceId, destinationCellId, critical, silentCast)
        {
            this.spellId = spellId;
            this.spellLevel = spellLevel;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(spellId);
            writer.WriteSByte(spellLevel);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            spellId = reader.ReadShort();
            if (spellId < 0)
                throw new Exception("Forbidden value on spellId = " + spellId + ", it doesn't respect the following condition : spellId < 0");
            spellLevel = reader.ReadSByte();
            if (spellLevel < 1 || spellLevel > 6)
                throw new Exception("Forbidden value on spellLevel = " + spellLevel + ", it doesn't respect the following condition : spellLevel < 1 || spellLevel > 6");
		}
	}
}
