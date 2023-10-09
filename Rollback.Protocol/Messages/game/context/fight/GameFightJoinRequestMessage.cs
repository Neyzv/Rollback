using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record GameFightJoinRequestMessage : Message
	{
        public int fightId;
        public int fighterId;

        public const int Id = 701;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameFightJoinRequestMessage()
        {
        }
        public GameFightJoinRequestMessage(int fightId, int fighterId)
        {
            this.fightId = fightId;
            this.fighterId = fighterId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(fightId);
            writer.WriteInt(fighterId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            fightId = reader.ReadInt();
            fighterId = reader.ReadInt();
		}
	}
}
