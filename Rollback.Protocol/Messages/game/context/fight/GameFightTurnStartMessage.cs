using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record GameFightTurnStartMessage : Message
	{
        public int id;
        public int waitTime;

        public const int Id = 714;
        public override uint MessageId
        {
        	get { return 714; }
        }
        public GameFightTurnStartMessage()
        {
        }
        public GameFightTurnStartMessage(int id, int waitTime)
        {
            this.id = id;
            this.waitTime = waitTime;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(id);
            writer.WriteInt(waitTime);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            id = reader.ReadInt();
            waitTime = reader.ReadInt();
            if (waitTime < 0)
                throw new Exception("Forbidden value on waitTime = " + waitTime + ", it doesn't respect the following condition : waitTime < 0");
		}
	}
}
