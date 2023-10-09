using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record InventoryWeightMessage : Message
	{
        public int weight;
        public int weightMax;

        public const int Id = 3009;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public InventoryWeightMessage()
        {
        }
        public InventoryWeightMessage(int weight, int weightMax)
        {
            this.weight = weight;
            this.weightMax = weightMax;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(weight);
            writer.WriteInt(weightMax);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            weight = reader.ReadInt();
            if (weight < 0)
                throw new Exception("Forbidden value on weight = " + weight + ", it doesn't respect the following condition : weight < 0");
            weightMax = reader.ReadInt();
            if (weightMax < 0)
                throw new Exception("Forbidden value on weightMax = " + weightMax + ", it doesn't respect the following condition : weightMax < 0");
		}
	}
}
