using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record FightTeamMemberMonsterInformations : FightTeamMemberInformations
    {
        public int monsterId;
        public sbyte grade;
        public new const short Id = 6;
        public override short TypeId
        {
            get { return Id; }
        }
        public FightTeamMemberMonsterInformations()
        {
        }
        public FightTeamMemberMonsterInformations(int id, int monsterId, sbyte grade) : base(id)
        {
            this.monsterId = monsterId;
            this.grade = grade;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(monsterId);
            writer.WriteSByte(grade);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            monsterId = reader.ReadInt();
            grade = reader.ReadSByte();
            if (grade < 0)
                throw new Exception("Forbidden value on grade = " + grade + ", it doesn't respect the following condition : grade < 0");
        }
    }
}
