using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record SkillActionDescriptionCollect : SkillActionDescriptionTimed
    {
        public short min;
        public short max;
        public new const short Id = 99;
        public override short TypeId
        {
            get { return Id; }
        }
        public SkillActionDescriptionCollect()
        {
        }
        public SkillActionDescriptionCollect(short skillId, byte time, short min, short max) : base(skillId, time)
        {
            this.min = min;
            this.max = max;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(min);
            writer.WriteShort(max);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            min = reader.ReadShort();
            if (min < 0)
                throw new Exception("Forbidden value on min = " + min + ", it doesn't respect the following condition : min < 0");
            max = reader.ReadShort();
            if (max < 0)
                throw new Exception("Forbidden value on max = " + max + ", it doesn't respect the following condition : max < 0");
        }
    }
}