using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Types
{
    public record GameRolePlayNamedActorInformations : GameRolePlayActorInformations
    {
        public string name;
        public new const short Id = 154;
        public override short TypeId
        {
            get { return Id; }
        }
        public GameRolePlayNamedActorInformations()
        {
        }
        public GameRolePlayNamedActorInformations(int contextualId, EntityLook look, EntityDispositionInformations disposition, string name) : base(contextualId, look, disposition)
        {
            this.name = name;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteString(name);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            name = reader.ReadString();
        }
    }
}
