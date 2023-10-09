using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record GameRolePlayRemoveChallengeMessage : Message
	{
        public int fightId;

        public const int Id = 300;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameRolePlayRemoveChallengeMessage()
        {
        }
        public GameRolePlayRemoveChallengeMessage(int fightId)
        {
            this.fightId = fightId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(fightId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            fightId = reader.ReadInt();
		}
	}
}
