using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record StatsUpgradeResultMessage : Message
	{
        public short nbCharacBoost;

        public const int Id = 5609;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public StatsUpgradeResultMessage()
        {
        }
        public StatsUpgradeResultMessage(short nbCharacBoost)
        {
            this.nbCharacBoost = nbCharacBoost;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(nbCharacBoost);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            nbCharacBoost = reader.ReadShort();
            if (nbCharacBoost < 0)
                throw new Exception("Forbidden value on nbCharacBoost = " + nbCharacBoost + ", it doesn't respect the following condition : nbCharacBoost < 0");
		}
	}
}
