using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;


namespace Rollback.Protocol.Messages
{
	public record JobLevelUpMessage : Message
	{
        public sbyte newLevel;
        public JobDescription jobsDescription;

        public const int Id = 5656;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public JobLevelUpMessage()
        {
        }
        public JobLevelUpMessage(sbyte newLevel, JobDescription jobsDescription)
        {
            this.newLevel = newLevel;
            this.jobsDescription = jobsDescription;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(newLevel);
            jobsDescription.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            newLevel = reader.ReadSByte();
            if (newLevel < 0)
                throw new Exception("Forbidden value on newLevel = " + newLevel + ", it doesn't respect the following condition : newLevel < 0");
            jobsDescription = new JobDescription();
            jobsDescription.Deserialize(reader);
		}
	}
}
