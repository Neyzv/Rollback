using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record SkillActionDescriptionTimed : SkillActionDescription
    {
        public byte time;
        public new const short Id = 103;
        public override short TypeId
        {
            get { return Id; }
        }
        public SkillActionDescriptionTimed()
        {
        }
        public SkillActionDescriptionTimed(short skillId, byte time) : base(skillId)
        {
            this.time = time;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteByte(time);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            time = reader.ReadByte();
            if (time < 0 || time > 255)
                throw new Exception("Forbidden value on time = " + time + ", it doesn't respect the following condition : time < 0 || time > 255");
        }
    }
}