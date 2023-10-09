using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record HouseInformationsForGuild
    {
        public int houseId;
        public string ownerName;
        public short worldX;
        public short worldY;
        public int[] skillListIds;
        public uint guildshareParams;
        public const short Id = 170;
        public virtual short TypeId
        {
            get { return Id; }
        }
        public HouseInformationsForGuild()
        {
        }
        public HouseInformationsForGuild(int houseId, string ownerName, short worldX, short worldY, int[] skillListIds, uint guildshareParams)
        {
            this.houseId = houseId;
            this.ownerName = ownerName;
            this.worldX = worldX;
            this.worldY = worldY;
            this.skillListIds = skillListIds;
            this.guildshareParams = guildshareParams;
        }
        public virtual void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(houseId);
            writer.WriteString(ownerName);
            writer.WriteShort(worldX);
            writer.WriteShort(worldY);
            writer.WriteUShort((ushort)skillListIds.Length);
            foreach (var entry in skillListIds)
            {
                writer.WriteInt(entry);
            }
            writer.WriteUInt(guildshareParams);
        }
        public virtual void Deserialize(BigEndianReader reader)
        {
            houseId = reader.ReadInt();
            if (houseId < 0)
                throw new Exception("Forbidden value on houseId = " + houseId + ", it doesn't respect the following condition : houseId < 0");
            ownerName = reader.ReadString();
            worldX = reader.ReadShort();
            if (worldX < -255 || worldX > 255)
                throw new Exception("Forbidden value on worldX = " + worldX + ", it doesn't respect the following condition : worldX < -255 || worldX > 255");
            worldY = reader.ReadShort();
            if (worldY < -255 || worldY > 255)
                throw new Exception("Forbidden value on worldY = " + worldY + ", it doesn't respect the following condition : worldY < -255 || worldY > 255");
            var limit = reader.ReadUShort();
            skillListIds = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                skillListIds[i] = reader.ReadInt();
            }
            guildshareParams = reader.ReadUInt();
            if (guildshareParams < 0 || guildshareParams > 4294967295)
                throw new Exception("Forbidden value on guildshareParams = " + guildshareParams + ", it doesn't respect the following condition : guildshareParams < 0 || guildshareParams > 4294967295");
        }
    }
}