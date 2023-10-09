using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record LivingObjectFeedMessage : Message
	{
        public int livingUID;
        public byte livingPosition;
        public int foodUID;

        public const int Id = 5724;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public LivingObjectFeedMessage()
        {
        }
        public LivingObjectFeedMessage(int livingUID, byte livingPosition, int foodUID)
        {
            this.livingUID = livingUID;
            this.livingPosition = livingPosition;
            this.foodUID = foodUID;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(livingUID);
            writer.WriteByte(livingPosition);
            writer.WriteInt(foodUID);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            livingUID = reader.ReadInt();
            if (livingUID < 0)
                throw new Exception("Forbidden value on livingUID = " + livingUID + ", it doesn't respect the following condition : livingUID < 0");
            livingPosition = reader.ReadByte();
            if (livingPosition < 0 || livingPosition > 255)
                throw new Exception("Forbidden value on livingPosition = " + livingPosition + ", it doesn't respect the following condition : livingPosition < 0 || livingPosition > 255");
            foodUID = reader.ReadInt();
            if (foodUID < 0)
                throw new Exception("Forbidden value on foodUID = " + foodUID + ", it doesn't respect the following condition : foodUID < 0");
		}
	}
}
