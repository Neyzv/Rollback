using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record ObjectJobAddedMessage : Message
	{
        public sbyte jobId;

        public const int Id = 6014;
        public override uint MessageId
        {
        	get { return 6014; }
        }
        public ObjectJobAddedMessage()
        {
        }
        public ObjectJobAddedMessage(sbyte jobId)
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
