using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record GuildPaddockBoughtMessage : Message
	{
        public short worldX;
        public short worldY;
        public sbyte nbMountMax;
        public sbyte nbItemMax;

        public const int Id = 5952;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GuildPaddockBoughtMessage()
        {
        }
        public GuildPaddockBoughtMessage(short worldX, short worldY, sbyte nbMountMax, sbyte nbItemMax)
        {
            this.worldX = worldX;
            this.worldY = worldY;
            this.nbMountMax = nbMountMax;
            this.nbItemMax = nbItemMax;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(worldX);
            writer.WriteShort(worldY);
            writer.WriteSByte(nbMountMax);
            writer.WriteSByte(nbItemMax);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            worldX = reader.ReadShort();
            if (worldX < -255 || worldX > 255)
                throw new Exception("Forbidden value on worldX = " + worldX + ", it doesn't respect the following condition : worldX < -255 || worldX > 255");
            worldY = reader.ReadShort();
            if (worldY < -255 || worldY > 255)
                throw new Exception("Forbidden value on worldY = " + worldY + ", it doesn't respect the following condition : worldY < -255 || worldY > 255");
            nbMountMax = reader.ReadSByte();
            if (nbMountMax < 0)
                throw new Exception("Forbidden value on nbMountMax = " + nbMountMax + ", it doesn't respect the following condition : nbMountMax < 0");
            nbItemMax = reader.ReadSByte();
            if (nbItemMax < 0)
                throw new Exception("Forbidden value on nbItemMax = " + nbItemMax + ", it doesn't respect the following condition : nbItemMax < 0");
		}
	}
}
