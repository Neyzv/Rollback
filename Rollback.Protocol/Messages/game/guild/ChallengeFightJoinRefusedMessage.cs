using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record ChallengeFightJoinRefusedMessage : Message
	{
        public int playerId;
        public sbyte reason;

        public const int Id = 5908;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ChallengeFightJoinRefusedMessage()
        {
        }
        public ChallengeFightJoinRefusedMessage(int playerId, sbyte reason)
        {
            this.playerId = playerId;
            this.reason = reason;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(playerId);
            writer.WriteSByte(reason);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            playerId = reader.ReadInt();
            if (playerId < 0)
                throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0");
            reason = reader.ReadSByte();
		}
	}
}
