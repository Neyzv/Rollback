using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record MapRunningFightDetailsRequestMessage : Message
	{
        public int fightId;

        public const int Id = 5750;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public MapRunningFightDetailsRequestMessage()
        {
        }
        public MapRunningFightDetailsRequestMessage(int fightId)
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
            if (fightId < 0)
                throw new Exception("Forbidden value on fightId = " + fightId + ", it doesn't respect the following condition : fightId < 0");
		}
	}
}
