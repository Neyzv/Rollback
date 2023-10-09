using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record StatsUpgradeRequestMessage : Message
	{
        public sbyte statId;
        public short boostPoint;

        public const int Id = 5610;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public StatsUpgradeRequestMessage()
        {
        }
        public StatsUpgradeRequestMessage(sbyte statId, short boostPoint)
        {
            this.statId = statId;
            this.boostPoint = boostPoint;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(statId);
            writer.WriteShort(boostPoint);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            statId = reader.ReadSByte();
            if (statId < 0)
                throw new Exception("Forbidden value on statId = " + statId + ", it doesn't respect the following condition : statId < 0");
            boostPoint = reader.ReadShort();
            if (boostPoint < 0)
                throw new Exception("Forbidden value on boostPoint = " + boostPoint + ", it doesn't respect the following condition : boostPoint < 0");
		}
	}
}
