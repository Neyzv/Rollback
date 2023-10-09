using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record OrientedObjectItemWithLookInRolePlay : ObjectItemWithLookInRolePlay
    {
        public sbyte direction;
        public new const short Id = 199;
        public override short TypeId
        {
            get { return Id; }
        }
        public OrientedObjectItemWithLookInRolePlay()
        {
        }
        public OrientedObjectItemWithLookInRolePlay(short cellId, short objectGID, EntityLook entityLook, sbyte direction) : base(cellId, objectGID, entityLook)
        {
            this.direction = direction;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteSByte(direction);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            direction = reader.ReadSByte();
            if (direction < 0)
                throw new Exception("Forbidden value on direction = " + direction + ", it doesn't respect the following condition : direction < 0");
        }
    }
}
