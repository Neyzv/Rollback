using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record CharacterSpellModification
    {
        public sbyte modificationType;
        public short spellId;
        public CharacterBaseCharacteristic value;
        public const short Id = 215;
        public virtual short TypeId
        {
            get { return Id; }
        }
        public CharacterSpellModification()
        {
        }
        public CharacterSpellModification(sbyte modificationType, short spellId, CharacterBaseCharacteristic value)
        {
            this.modificationType = modificationType;
            this.spellId = spellId;
            this.value = value;
        }
        public virtual void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(modificationType);
            writer.WriteShort(spellId);
            value.Serialize(writer);
        }
        public virtual void Deserialize(BigEndianReader reader)
        {
            modificationType = reader.ReadSByte();
            if (modificationType < 0)
                throw new Exception("Forbidden value on modificationType = " + modificationType + ", it doesn't respect the following condition : modificationType < 0");
            spellId = reader.ReadShort();
            if (spellId < 0)
                throw new Exception("Forbidden value on spellId = " + spellId + ", it doesn't respect the following condition : spellId < 0");
            value = new CharacterBaseCharacteristic();
            value.Deserialize(reader);
        }
    }
}


