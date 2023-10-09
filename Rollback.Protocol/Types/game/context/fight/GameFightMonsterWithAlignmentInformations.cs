using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Types
{
    public record GameFightMonsterWithAlignmentInformations : GameFightMonsterInformations
    {
        public ActorAlignmentInformations alignmentInfos;

        public new const short Id = 203;
        public override short TypeId
        {
            get { return Id; }
        }
        public GameFightMonsterWithAlignmentInformations()
        {
        }
        public GameFightMonsterWithAlignmentInformations(int contextualId, EntityLook look, EntityDispositionInformations disposition, sbyte teamId, bool alive, GameFightMinimalStats stats, short creatureGenericId, sbyte creatureGrade, ActorAlignmentInformations alignmentInfos) : base(contextualId, look, disposition, teamId, alive, stats, creatureGenericId, creatureGrade)
        {
            this.alignmentInfos = alignmentInfos;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            alignmentInfos.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            alignmentInfos = new Types.ActorAlignmentInformations();
            alignmentInfos.Deserialize(reader);
        }
    }
}
