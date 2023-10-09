using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record HouseInformations
    {
        public int houseId;
        public int[] doorsOnMap;
        public string ownerName;
        public bool isOnSale;
        public short modelId;
        public const short Id = 111;
        public virtual short TypeId
        {
            get { return Id; }
        }
        public HouseInformations()
        {
        }
        public HouseInformations(int houseId, int[] doorsOnMap, string ownerName, bool isOnSale, short modelId)
        {
            this.houseId = houseId;
            this.doorsOnMap = doorsOnMap;
            this.ownerName = ownerName;
            this.isOnSale = isOnSale;
            this.modelId = modelId;
        }
        public virtual void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(houseId);
            writer.WriteUShort((ushort)doorsOnMap.Length);
            foreach (var entry in doorsOnMap)
            {
                writer.WriteInt(entry);
            }
            writer.WriteString(ownerName);
            writer.WriteBoolean(isOnSale);
            writer.WriteShort(modelId);
        }
        public virtual void Deserialize(BigEndianReader reader)
        {
            houseId = reader.ReadInt();
            if (houseId < 0)
                throw new Exception("Forbidden value on houseId = " + houseId + ", it doesn't respect the following condition : houseId < 0");
            var limit = reader.ReadUShort();
            doorsOnMap = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                doorsOnMap[i] = reader.ReadInt();
            }
            ownerName = reader.ReadString();
            isOnSale = reader.ReadBoolean();
            modelId = reader.ReadShort();
            if (modelId < 0)
                throw new Exception("Forbidden value on modelId = " + modelId + ", it doesn't respect the following condition : modelId < 0");
        }
    }
}
