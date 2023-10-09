using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record JobDescriptionMessage : Message
	{
        public JobDescription[] jobsDescription;

        public const int Id = 5655;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public JobDescriptionMessage()
        {
        }
        public JobDescriptionMessage(JobDescription[] jobsDescription)
        {
            this.jobsDescription = jobsDescription;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)jobsDescription.Length);
            foreach (var entry in jobsDescription)
            {
                 entry.Serialize(writer);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            jobsDescription = new JobDescription[limit];
            for (int i = 0; i < limit; i++)
            {
                 jobsDescription[i] = new JobDescription();
                 jobsDescription[i].Deserialize(reader);
            }
		}
	}
}
