using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record JobUnlearntMessage : Message
	{
        public sbyte jobId;

        public const int Id = 5657;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public JobUnlearntMessage()
        {
        }
        public JobUnlearntMessage(sbyte jobId)
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
