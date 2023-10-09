using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record CharacterMinimalPlusLookAndGradeInformations : CharacterMinimalPlusLookInformations
    {
        public int grade;
        public new const short Id = 193;
        public override short TypeId
        {
            get { return Id; }
        }
        public CharacterMinimalPlusLookAndGradeInformations()
        {
        }
        public CharacterMinimalPlusLookAndGradeInformations(int id, string name, byte level, Types.EntityLook entityLook, int grade) : base(id, name, level, entityLook)
        {
            this.grade = grade;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(grade);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            grade = reader.ReadInt();
            if (grade < 0)
                throw new Exception("Forbidden value on grade = " + grade + ", it doesn't respect the following condition : grade < 0");
        }
    }
}
