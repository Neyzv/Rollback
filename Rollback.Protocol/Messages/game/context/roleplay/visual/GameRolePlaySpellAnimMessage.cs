using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record GameRolePlaySpellAnimMessage : Message
	{
        public int casterId;
        public short targetCellId;
        public short spellId;
        public sbyte spellLevel;

        public const int Id = 6114;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameRolePlaySpellAnimMessage()
        {
        }
        public GameRolePlaySpellAnimMessage(int casterId, short targetCellId, short spellId, sbyte spellLevel)
        {
            this.casterId = casterId;
            this.targetCellId = targetCellId;
            this.spellId = spellId;
            this.spellLevel = spellLevel;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(casterId);
            writer.WriteShort(targetCellId);
            writer.WriteShort(spellId);
            writer.WriteSByte(spellLevel);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            casterId = reader.ReadInt();
            targetCellId = reader.ReadShort();
            if (targetCellId < 0 || targetCellId > 559)
                throw new Exception("Forbidden value on targetCellId = " + targetCellId + ", it doesn't respect the following condition : targetCellId < 0 || targetCellId > 559");
            spellId = reader.ReadShort();
            if (spellId < 0)
                throw new Exception("Forbidden value on spellId = " + spellId + ", it doesn't respect the following condition : spellId < 0");
            spellLevel = reader.ReadSByte();
            if (spellLevel < 1 || spellLevel > 6)
                throw new Exception("Forbidden value on spellLevel = " + spellLevel + ", it doesn't respect the following condition : spellLevel < 1 || spellLevel > 6");
		}
	}
}
