using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Types
{
    public record GameRolePlayActorInformations : GameContextActorInformations
    {
        public new const short Id = 141;
        public override short TypeId
        {
            get { return Id; }
        }
        public GameRolePlayActorInformations()
        {
        }
        public GameRolePlayActorInformations(int contextualId, EntityLook look, EntityDispositionInformations disposition) : base(contextualId, look, disposition)
        {
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
        }
    }
}
