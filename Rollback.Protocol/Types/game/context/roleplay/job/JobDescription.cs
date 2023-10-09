using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record JobDescription
    {
        public sbyte jobId;
        public SkillActionDescription[] skills;
        public const short Id = 101;
        public virtual short TypeId
        {
            get { return Id; }
        }
        public JobDescription()
        {
        }
        public JobDescription(sbyte jobId, SkillActionDescription[] skills)
        {
            this.jobId = jobId;
            this.skills = skills;
        }
        public virtual void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(jobId);
            writer.WriteUShort((ushort)skills.Length);
            foreach (var entry in skills)
            {
                writer.WriteShort(entry.TypeId);
                entry.Serialize(writer);
            }
        }
        public virtual void Deserialize(BigEndianReader reader)
        {
            jobId = reader.ReadSByte();
            if (jobId < 0)
                throw new Exception("Forbidden value on jobId = " + jobId + ", it doesn't respect the following condition : jobId < 0");
            var limit = reader.ReadUShort();
            skills = new Types.SkillActionDescription[limit];
            for (int i = 0; i < limit; i++)
            {
                skills[i] = (SkillActionDescription)ProtocolManager.Instance.GetType(reader.ReadUShort());
                skills[i].Deserialize(reader);
            }
        }
    }
}
