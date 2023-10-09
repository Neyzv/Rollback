using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record ObjectEffectLadder : ObjectEffectCreature
    {
        public int monsterCount;
        public new const short Id = 81;
        public override short TypeId
        {
            get { return Id; }
        }
        public ObjectEffectLadder()
        {
        }
        public ObjectEffectLadder(short actionId, short monsterFamilyId, int monsterCount) : base(actionId, monsterFamilyId)
        {
            this.monsterCount = monsterCount;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(monsterCount);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            monsterCount = reader.ReadInt();
            if (monsterCount < 0)
                throw new Exception("Forbidden value on monsterCount = " + monsterCount + ", it doesn't respect the following condition : monsterCount < 0");
        }
    }
}
