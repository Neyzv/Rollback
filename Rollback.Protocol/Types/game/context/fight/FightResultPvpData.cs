
using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Types
{
    public record FightResultPvpData : FightResultAdditionalData
    {
        public byte grade;
        public ushort minHonorForGrade;
        public ushort maxHonorForGrade;
        public ushort honor;
        public short honorDelta;
        public ushort dishonor;
        public short dishonorDelta;
        public new const short Id = 190;
        public override short TypeId
        {
            get { return Id; }
        }
        public FightResultPvpData()
        {
        }
        public FightResultPvpData(byte grade, ushort minHonorForGrade, ushort maxHonorForGrade, ushort honor, short honorDelta, ushort dishonor, short dishonorDelta)
        {
            this.grade = grade;
            this.minHonorForGrade = minHonorForGrade;
            this.maxHonorForGrade = maxHonorForGrade;
            this.honor = honor;
            this.honorDelta = honorDelta;
            this.dishonor = dishonor;
            this.dishonorDelta = dishonorDelta;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteByte(grade);
            writer.WriteUShort(minHonorForGrade);
            writer.WriteUShort(maxHonorForGrade);
            writer.WriteUShort(honor);
            writer.WriteShort(honorDelta);
            writer.WriteUShort(dishonor);
            writer.WriteShort(dishonorDelta);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            grade = reader.ReadByte();
            if (grade < 0 || grade > 255)
                throw new Exception("Forbidden value on grade = " + grade + ", it doesn't respect the following condition : grade < 0 || grade > 255");
            minHonorForGrade = reader.ReadUShort();
            if (minHonorForGrade < 0 || minHonorForGrade > 18000)
                throw new Exception("Forbidden value on minHonorForGrade = " + minHonorForGrade + ", it doesn't respect the following condition : minHonorForGrade < 0 || minHonorForGrade > 18000");
            maxHonorForGrade = reader.ReadUShort();
            if (maxHonorForGrade < 0 || maxHonorForGrade > 18000)
                throw new Exception("Forbidden value on maxHonorForGrade = " + maxHonorForGrade + ", it doesn't respect the following condition : maxHonorForGrade < 0 || maxHonorForGrade > 18000");
            honor = reader.ReadUShort();
            if (honor < 0 || honor > 18000)
                throw new Exception("Forbidden value on honor = " + honor + ", it doesn't respect the following condition : honor < 0 || honor > 18000");
            honorDelta = reader.ReadShort();
            dishonor = reader.ReadUShort();
            if (dishonor < 0 || dishonor > 500)
                throw new Exception("Forbidden value on dishonor = " + dishonor + ", it doesn't respect the following condition : dishonor < 0 || dishonor > 500");
            dishonorDelta = reader.ReadShort();
        }
    }
}
