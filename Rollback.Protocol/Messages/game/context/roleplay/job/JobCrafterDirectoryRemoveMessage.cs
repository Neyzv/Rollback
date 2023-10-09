using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record JobCrafterDirectoryRemoveMessage : Message
	{
        public sbyte jobId;
        public int playerId;

        public const int Id = 5653;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public JobCrafterDirectoryRemoveMessage()
        {
        }
        public JobCrafterDirectoryRemoveMessage(sbyte jobId, int playerId)
        {
            this.jobId = jobId;
            this.playerId = playerId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(jobId);
            writer.WriteInt(playerId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            jobId = reader.ReadSByte();
            if (jobId < 0)
                throw new Exception("Forbidden value on jobId = " + jobId + ", it doesn't respect the following condition : jobId < 0");
            playerId = reader.ReadInt();
            if (playerId < 0)
                throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0");
		}
	}
}
