using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record ChallengeResultMessage : Message
	{
        public sbyte challengeId;
        public bool success;

        public const int Id = 6019;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ChallengeResultMessage()
        {
        }
        public ChallengeResultMessage(sbyte challengeId, bool success)
        {
            this.challengeId = challengeId;
            this.success = success;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(challengeId);
            writer.WriteBoolean(success);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            challengeId = reader.ReadSByte();
            if (challengeId < 0)
                throw new Exception("Forbidden value on challengeId = " + challengeId + ", it doesn't respect the following condition : challengeId < 0");
            success = reader.ReadBoolean();
		}
	}
}
