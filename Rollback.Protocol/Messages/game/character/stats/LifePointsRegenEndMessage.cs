using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record LifePointsRegenEndMessage : UpdateLifePointsMessage
	{
        public int lifePointsGained;

        public new const int Id = 5686;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public LifePointsRegenEndMessage()
        {
        }
        public LifePointsRegenEndMessage(int lifePoints, int maxLifePoints, int lifePointsGained) : base(lifePoints, maxLifePoints)
        {
            this.lifePointsGained = lifePointsGained;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(lifePointsGained);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            lifePointsGained = reader.ReadInt();
            if (lifePointsGained < 0)
                throw new Exception("Forbidden value on lifePointsGained = " + lifePointsGained + ", it doesn't respect the following condition : lifePointsGained < 0");
		}
	}
}
