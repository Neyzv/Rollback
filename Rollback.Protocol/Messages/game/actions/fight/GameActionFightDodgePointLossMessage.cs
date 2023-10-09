using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record GameActionFightDodgePointLossMessage : AbstractGameActionMessage
	{
        public int targetId;
        public short amount;

        public new const int Id = 5828;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameActionFightDodgePointLossMessage()
        {
        }
        public GameActionFightDodgePointLossMessage(short actionId, int sourceId, int targetId, short amount) : base(actionId, sourceId)
        {
            this.targetId = targetId;
            this.amount = amount;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(targetId);
            writer.WriteShort(amount);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            targetId = reader.ReadInt();
            amount = reader.ReadShort();
            if (amount < 0)
                throw new Exception("Forbidden value on amount = " + amount + ", it doesn't respect the following condition : amount < 0");
		}
	}
}
