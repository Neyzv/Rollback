using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record PVPActivationCostMessage : Message
	{
        public short cost;

        public const int Id = 1801;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public PVPActivationCostMessage()
        {
        }
        public PVPActivationCostMessage(short cost)
        {
            this.cost = cost;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(cost);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            cost = reader.ReadShort();
            if (cost < 0)
                throw new Exception("Forbidden value on cost = " + cost + ", it doesn't respect the following condition : cost < 0");
		}
	}
}
