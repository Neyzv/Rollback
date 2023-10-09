using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record JobExperienceUpdateMessage : Message
	{
        public JobExperience experiencesUpdate;

        public const int Id = 0x1616;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public JobExperienceUpdateMessage()
        {
        }
        public JobExperienceUpdateMessage(JobExperience experiencesUpdate)
        {
            this.experiencesUpdate = experiencesUpdate;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            experiencesUpdate.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            experiencesUpdate = new JobExperience();
            experiencesUpdate.Deserialize(reader);
		}
	}
}
