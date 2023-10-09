using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Types
{
    public record ObjectItemWithLookInRolePlay : ObjectItemInRolePlay
    {
        public EntityLook entityLook;
        public new const short Id = 197;
        public override short TypeId
        {
            get { return Id; }
        }
        public ObjectItemWithLookInRolePlay()
        {
        }
        public ObjectItemWithLookInRolePlay(short cellId, short objectGID, EntityLook entityLook) : base(cellId, objectGID)
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
