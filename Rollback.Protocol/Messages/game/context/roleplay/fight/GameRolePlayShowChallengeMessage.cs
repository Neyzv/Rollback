using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record GameRolePlayShowChallengeMessage : Message
	{
        public FightCommonInformations commonsInfos;

        public const int Id = 301;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameRolePlayShowChallengeMessage()
        {
        }
        public GameRolePlayShowChallengeMessage(FightCommonInformations commonsInfos)
        {
            this.commonsInfos = commonsInfos;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            commonsInfos.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            commonsInfos = new FightCommonInformations();
            commonsInfos.Deserialize(reader);
		}
	}
}
