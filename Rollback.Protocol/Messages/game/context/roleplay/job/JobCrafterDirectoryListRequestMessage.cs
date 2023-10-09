using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record JobCrafterDirectoryListRequestMessage : Message
	{
        public sbyte jobId;

        public const int Id = 6047;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public JobCrafterDirectoryListRequestMessage()
        {
        }
        public JobCrafterDirectoryListRequestMessage(sbyte jobId)
        {
            this.jobId = jobId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(jobId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            jobId = reader.ReadSByte();
            if (jobId < 0)
                throw new Exception("Forbidden value on jobId = " + jobId + ", it doesn't respect the following condition : jobId < 0");
		}
	}
}
