using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record ExchangeMountFreeFromPaddockMessage : Message
	{
        public string name;
        public short worldX;
        public short worldY;
        public string liberator;

        public const int Id = 6055;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeMountFreeFromPaddockMessage()
        {
        }
        public ExchangeMountFreeFromPaddockMessage(string name, short worldX, short worldY, string liberator)
        {
            this.name = name;
            this.worldX = worldX;
            this.worldY = worldY;
            this.liberator = liberator;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteString(name);
            writer.WriteShort(worldX);
            writer.WriteShort(worldY);
            writer.WriteString(liberator);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            name = reader.ReadString();
            worldX = reader.ReadShort();
            if (worldX < -255 || worldX > 255)
                throw new Exception("Forbidden value on worldX = " + worldX + ", it doesn't respect the following condition : worldX < -255 || worldX > 255");
            worldY = reader.ReadShort();
            if (worldY < -255 || worldY > 255)
                throw new Exception("Forbidden value on worldY = " + worldY + ", it doesn't respect the following condition : worldY < -255 || worldY > 255");
            liberator = reader.ReadString();
		}
	}
}
