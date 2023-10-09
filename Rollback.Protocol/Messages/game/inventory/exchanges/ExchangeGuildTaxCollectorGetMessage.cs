using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;


namespace Rollback.Protocol.Messages
{
	public record ExchangeGuildTaxCollectorGetMessage : Message
	{
        public string collectorName;
        public short worldX;
        public short worldY;
        public int mapId;
        public string userName;
        public double experience;
        public ObjectItemQuantity[] objectsInfos;

        public const int Id = 5762;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeGuildTaxCollectorGetMessage()
        {
        }
        public ExchangeGuildTaxCollectorGetMessage(string collectorName, short worldX, short worldY, int mapId, string userName, double experience, ObjectItemQuantity[] objectsInfos)
        {
            this.collectorName = collectorName;
            this.worldX = worldX;
            this.worldY = worldY;
            this.mapId = mapId;
            this.userName = userName;
            this.experience = experience;
            this.objectsInfos = objectsInfos;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteString(collectorName);
            writer.WriteShort(worldX);
            writer.WriteShort(worldY);
            writer.WriteInt(mapId);
            writer.WriteString(userName);
            writer.WriteDouble(experience);
            writer.WriteUShort((ushort)objectsInfos.Length);
            foreach (var entry in objectsInfos)
            {
                 entry.Serialize(writer);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            collectorName = reader.ReadString();
            worldX = reader.ReadShort();
            if (worldX < -255 || worldX > 255)
                throw new Exception("Forbidden value on worldX = " + worldX + ", it doesn't respect the following condition : worldX < -255 || worldX > 255");
            worldY = reader.ReadShort();
            if (worldY < -255 || worldY > 255)
                throw new Exception("Forbidden value on worldY = " + worldY + ", it doesn't respect the following condition : worldY < -255 || worldY > 255");
            mapId = reader.ReadInt();
            if (mapId < 0)
                throw new Exception("Forbidden value on mapId = " + mapId + ", it doesn't respect the following condition : mapId < 0");
            userName = reader.ReadString();
            experience = reader.ReadDouble();
            var limit = reader.ReadUShort();
            objectsInfos = new ObjectItemQuantity[limit];
            for (int i = 0; i < limit; i++)
            {
                 objectsInfos[i] = new ObjectItemQuantity();
                 objectsInfos[i].Deserialize(reader);
            }
		}
	}
}
