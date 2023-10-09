using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record JobExperienceMultiUpdateMessage : Message
	{
        public JobExperience[] experiencesUpdate;

        public const int Id = 5809;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public JobExperienceMultiUpdateMessage()
        {
        }
        public JobExperienceMultiUpdateMessage(JobExperience[] experiencesUpdate)
        {
            this.experiencesUpdate = experiencesUpdate;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)experiencesUpdate.Length);
            foreach (var entry in experiencesUpdate)
            {
                 entry.Serialize(writer);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            experiencesUpdate = new JobExperience[limit];
            for (int i = 0; i < limit; i++)
            {
                 experiencesUpdate[i] = new JobExperience();
                 experiencesUpdate[i].Deserialize(reader);
            }
		}
	}
}
