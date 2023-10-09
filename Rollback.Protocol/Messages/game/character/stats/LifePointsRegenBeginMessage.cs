using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record LifePointsRegenBeginMessage : Message
	{
        public byte regenRate;

        public const int Id = 5684;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public LifePointsRegenBeginMessage()
        {
        }
        public LifePointsRegenBeginMessage(byte regenRate)
        {
            this.regenRate = regenRate;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteByte(regenRate);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            regenRate = reader.ReadByte();
            if (regenRate < 0 || regenRate > 255)
                throw new Exception("Forbidden value on regenRate = " + regenRate + ", it doesn't respect the following condition : regenRate < 0 || regenRate > 255");
		}
	}
}
