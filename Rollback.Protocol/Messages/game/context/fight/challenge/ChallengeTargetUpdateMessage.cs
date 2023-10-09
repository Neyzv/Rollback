using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record ChallengeTargetUpdateMessage : Message
	{
        public sbyte challengeId;
        public int targetId;

        public const int Id = 6123;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ChallengeTargetUpdateMessage()
        {
        }
        public ChallengeTargetUpdateMessage(sbyte challengeId, int targetId)
        {
            this.challengeId = challengeId;
            this.targetId = targetId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(challengeId);
            writer.WriteInt(targetId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            challengeId = reader.ReadSByte();
            if (challengeId < 0)
                throw new Exception("Forbidden value on challengeId = " + challengeId + ", it doesn't respect the following condition : challengeId < 0");
            targetId = reader.ReadInt();
		}
	}
}
