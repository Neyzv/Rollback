using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Types
{
    public record CharacterMinimalPlusLookInformations : CharacterMinimalInformations
    {
        public EntityLook entityLook;
        public new const short Id = 163;
        public override short TypeId
        {
            get { return Id; }
        }
        public CharacterMinimalPlusLookInformations()
        {
        }
        public CharacterMinimalPlusLookInformations(int id, string name, byte level, EntityLook entityLook) : base(id, name, level)
        {
            this.entityLook = entityLook;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            entityLook.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            entityLook = new EntityLook();
            entityLook.Deserialize(reader);
        }
    }
}

