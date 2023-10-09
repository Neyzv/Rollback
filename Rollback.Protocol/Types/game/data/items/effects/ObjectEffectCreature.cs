using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record ObjectEffectCreature : ObjectEffect
    {
        public short monsterFamilyId;
        public new const short Id = 71;
        public override short TypeId
        {
            get { return Id; }
        }
        public ObjectEffectCreature()
        {
        }
        public ObjectEffectCreature(short actionId, short monsterFamilyId) : base(actionId)
        {
            this.monsterFamilyId = monsterFamilyId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(monsterFamilyId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            monsterFamilyId = reader.ReadShort();
            if (monsterFamilyId < 0)
                throw new Exception("Forbidden value on monsterFamilyId = " + monsterFamilyId + ", it doesn't respect the following condition : monsterFamilyId < 0");
        }
    }
}
