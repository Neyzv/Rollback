using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record GameActionFightReduceDamagesMessage : AbstractGameActionMessage
	{
        public int targetId;
        public int amount;

        public new const int Id = 5526;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameActionFightReduceDamagesMessage()
        {
        }
        public GameActionFightReduceDamagesMessage(short actionId, int sourceId, int targetId, int amount) : base(actionId, sourceId)
        {
            this.targetId = targetId;
            this.amount = amount;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(targetId);
            writer.WriteInt(amount);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            targetId = reader.ReadInt();
            amount = reader.ReadInt();
            if (amount < 0)
                throw new Exception("Forbidden value on amount = " + amount + ", it doesn't respect the following condition : amount < 0");
		}
	}
}
