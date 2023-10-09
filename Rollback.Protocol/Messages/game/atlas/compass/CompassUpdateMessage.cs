using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record CompassUpdateMessage : Message
	{
        public sbyte type;
        public short worldX;
        public short worldY;

        public const int Id = 5591;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public CompassUpdateMessage()
        {
        }
        public CompassUpdateMessage(sbyte type, short worldX, short worldY)
        {
            this.type = type;
            this.worldX = worldX;
            this.worldY = worldY;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(type);
            writer.WriteShort(worldX);
            writer.WriteShort(worldY);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            type = reader.ReadSByte();
            if (type < 0)
                throw new Exception("Forbidden value on type = " + type + ", it doesn't respect the following condition : type < 0");
            worldX = reader.ReadShort();
            if (worldX < -255 || worldX > 255)
                throw new Exception("Forbidden value on worldX = " + worldX + ", it doesn't respect the following condition : worldX < -255 || worldX > 255");
            worldY = reader.ReadShort();
            if (worldY < -255 || worldY > 255)
                throw new Exception("Forbidden value on worldY = " + worldY + ", it doesn't respect the following condition : worldY < -255 || worldY > 255");
		}
	}
}
