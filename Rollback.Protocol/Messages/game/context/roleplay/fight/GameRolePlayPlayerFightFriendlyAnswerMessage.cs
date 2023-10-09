using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record GameRolePlayPlayerFightFriendlyAnswerMessage : Message
	{
        public int fightId;
        public bool accept;

        public const int Id = 5732;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameRolePlayPlayerFightFriendlyAnswerMessage()
        {
        }
        public GameRolePlayPlayerFightFriendlyAnswerMessage(int fightId, bool accept)
        {
            this.fightId = fightId;
            this.accept = accept;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(fightId);
            writer.WriteBoolean(accept);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            fightId = reader.ReadInt();
            accept = reader.ReadBoolean();
		}
	}
}
