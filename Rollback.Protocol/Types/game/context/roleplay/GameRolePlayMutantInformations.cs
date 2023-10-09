using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Types
{
    public record GameRolePlayMutantInformations : GameRolePlayHumanoidInformations
    {
        public sbyte powerLevel;
        public new const short Id = 3;
        public override short TypeId
        {
            get { return Id; }
        }
        public GameRolePlayMutantInformations()
        {
        }
        public GameRolePlayMutantInformations(int contextualId, EntityLook look, EntityDispositionInformations disposition, string name, HumanInformations humanoidInfo, sbyte powerLevel) : base(contextualId, look, disposition, name, humanoidInfo)
        {
            this.powerLevel = powerLevel;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteSByte(powerLevel);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            powerLevel = reader.ReadSByte();
        }
    }
}
