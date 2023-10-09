using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Types
{
    public record GameRolePlayHumanoidInformations : GameRolePlayNamedActorInformations
    {
        public HumanInformations humanoidInfo;
        public new const short Id = 159;
        public override short TypeId
        {
            get { return Id; }
        }
        public GameRolePlayHumanoidInformations()
        {
        }
        public GameRolePlayHumanoidInformations(int contextualId, EntityLook look, EntityDispositionInformations disposition, string name, HumanInformations humanoidInfo) : base(contextualId, look, disposition, name)
        {
            this.humanoidInfo = humanoidInfo;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(humanoidInfo.TypeId);
            humanoidInfo.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            humanoidInfo = (HumanInformations)ProtocolManager.Instance.GetType(reader.ReadUShort());
            humanoidInfo.Deserialize(reader);
        }
    }
}
