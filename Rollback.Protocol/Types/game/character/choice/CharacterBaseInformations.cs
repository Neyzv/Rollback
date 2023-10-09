using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Types
{
    public record CharacterBaseInformations : CharacterMinimalPlusLookInformations
    {
        public sbyte breed;
        public bool sex;
        public new const short Id = 45;
        public override short TypeId
        {
            get { return Id; }
        }
        public CharacterBaseInformations()
        {
        }
        public CharacterBaseInformations(int id, string name, byte level, Types.EntityLook entityLook, sbyte breed, bool sex) : base(id, name, level, entityLook)
        {
            this.breed = breed;
            this.sex = sex;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteSByte(breed);
            writer.WriteBoolean(sex);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            breed = reader.ReadSByte();
            sex = reader.ReadBoolean();
        }
    }
}
