using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Types
{
    public record GameFightAIInformations : GameFightFighterInformations
    {
        public new const short Id = 151;
        public override short TypeId
        {
            get { return Id; }
        }
        public GameFightAIInformations()
        {
        }
        public GameFightAIInformations(int contextualId, EntityLook look, EntityDispositionInformations disposition, sbyte teamId, bool alive, GameFightMinimalStats stats) : base(contextualId, look, disposition, teamId, alive, stats)
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
