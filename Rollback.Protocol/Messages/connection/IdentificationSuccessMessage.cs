using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record IdentificationSuccessMessage : Message
	{
        public bool hasRights;
        public bool wasAlreadyConnected;
        public string nickname;
        public sbyte communityId;
        public string secretQuestion;
        public double remainingSubscriptionTime;

        public const int Id = 22;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public IdentificationSuccessMessage()
        {
        }
        public IdentificationSuccessMessage(bool hasRights, bool wasAlreadyConnected, string nickname, sbyte communityId, string secretQuestion, double remainingSubscriptionTime)
        {
            this.hasRights = hasRights;
            this.wasAlreadyConnected = wasAlreadyConnected;
            this.nickname = nickname;
            this.communityId = communityId;
            this.secretQuestion = secretQuestion;
            this.remainingSubscriptionTime = remainingSubscriptionTime;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            byte flag1 = 0;
            flag1 = BigEndianBooleanByteWrapper.SetFlag(flag1, 0, hasRights);
            flag1 = BigEndianBooleanByteWrapper.SetFlag(flag1, 1, wasAlreadyConnected);
            writer.WriteByte(flag1);
            writer.WriteString(nickname);
            writer.WriteSByte(communityId);
            writer.WriteString(secretQuestion);
            writer.WriteDouble(remainingSubscriptionTime);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            byte flag1 = reader.ReadByte();
            hasRights = BigEndianBooleanByteWrapper.GetFlag(flag1, 0);
            wasAlreadyConnected = BigEndianBooleanByteWrapper.GetFlag(flag1, 1);
            nickname = reader.ReadString();
            communityId = reader.ReadSByte();
            if (communityId < 0)
                throw new Exception("Forbidden value on communityId = " + communityId + ", it doesn't respect the following condition : communityId < 0");
            secretQuestion = reader.ReadString();
            remainingSubscriptionTime = reader.ReadDouble();
            if (remainingSubscriptionTime < 0)
                throw new Exception("Forbidden value on remainingSubscriptionTime = " + remainingSubscriptionTime + ", it doesn't respect the following condition : remainingSubscriptionTime < 0");
		}
	}
}
