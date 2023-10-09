using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record SkillActionDescription
    {
        public short skillId;
        public const short Id = 102;
        public virtual short TypeId
        {
            get { return Id; }
        }
        public SkillActionDescription()
        {
        }
        public SkillActionDescription(short skillId)
        {
            this.skillId = skillId;
        }
        public virtual void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(skillId);
        }
        public virtual void Deserialize(BigEndianReader reader)
        {
            skillId = reader.ReadShort();
            if (skillId < 0)
                throw new Exception("Forbidden value on skillId = " + skillId + ", it doesn't respect the following condition : skillId < 0");
        }
    }
}