using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record ChallengeTargetsListRequestMessage : Message
	{
        public sbyte challengeId;

        public const int Id = 5614;
        public override uint MessageId
        {
        	get { return 5614; }
        }
        public ChallengeTargetsListRequestMessage()
        {
        }
        public ChallengeTargetsListRequestMessage(sbyte challengeId)
        {
            this.challengeId = challengeId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(challengeId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            challengeId = reader.ReadSByte();
            if (challengeId < 0)
                throw new Exception("Forbidden value on challengeId = " + challengeId + ", it doesn't respect the following condition : challengeId < 0");
		}
	}
}
