using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Types
{
    public record GameRolePlayPrismInformations : GameRolePlayActorInformations
    {
        public ActorAlignmentInformations alignInfos;
        public new const short Id = 161;
        public override short TypeId
        {
            get { return Id; }
        }
        public GameRolePlayPrismInformations()
        {
        }
        public GameRolePlayPrismInformations(int contextualId, EntityLook look, EntityDispositionInformations disposition, ActorAlignmentInformations alignInfos) : base(contextualId, look, disposition)
        {
            this.alignInfos = alignInfos;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            alignInfos.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            alignInfos = new ActorAlignmentInformations();
            alignInfos.Deserialize(reader);
        }
    }
}
