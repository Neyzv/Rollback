using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record MapCoordinatesExtended : MapCoordinates
    {
        public int mapId;
        public new const short Id = 176;
        public override short TypeId
        {
            get { return Id; }
        }
        public MapCoordinatesExtended()
        {
        }
        public MapCoordinatesExtended(short worldX, short worldY, int mapId) : base(worldX, worldY)
        {
            this.mapId = mapId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(mapId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            mapId = reader.ReadInt();
            if (mapId < 0)
                throw new Exception("Forbidden value on mapId = " + mapId + ", it doesn't respect the following condition : mapId < 0");
        }
    }
}
