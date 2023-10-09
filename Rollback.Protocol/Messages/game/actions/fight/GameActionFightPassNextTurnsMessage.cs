using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record GameActionFightPassNextTurnsMessage : AbstractGameActionMessage
	{
        public int targetId;
        public sbyte turnCount;

        public new const int Id = 5529;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameActionFightPassNextTurnsMessage()
        {
        }
        public GameActionFightPassNextTurnsMessage(short actionId, int sourceId, int targetId, sbyte turnCount) : base(actionId, sourceId)
        {
            this.targetId = targetId;
            this.turnCount = turnCount;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(targetId);
            writer.WriteSByte(turnCount);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            targetId = reader.ReadInt();
            turnCount = reader.ReadSByte();
            if (turnCount < 0)
                throw new Exception("Forbidden value on turnCount = " + turnCount + ", it doesn't respect the following condition : turnCount < 0");
		}
	}
}
