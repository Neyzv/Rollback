using Rollback.Common.IO.Binary;

namespace Rollback.Client.Dlm
{
    public sealed class Map
    {
        private const short cellsCount = 560;

        #region Properties
        public sbyte MapVersion { get; set; }

        public int Id { get; set; }

        public int RelativeId { get; set; }

        public sbyte MapType { get; set; }

        public int SubAreaId { get; set; }

        public int TopNeighbourId { get; set; }

        public int BottomNeighbourId { get; set; }

        public int LeftNeighbourId { get; set; }

        public int RightNeighbourId { get; set; }

        public int ShadowBonusOnEntities { get; set; }

        public Fixture[] BackgroundFixtures { get; set; }

        public Fixture[] ForegroundFixtures { get; set; }

        public int GroundCRC { get; set; }

        public int Checksum { get; set; }

        public Layer[] Layers { get; set; }

        public CellData[] CellDatas { get; set; }
        #endregion

        public Map()
        {
            BackgroundFixtures = Array.Empty<Fixture>();
            ForegroundFixtures = Array.Empty<Fixture>();
            Layers = Array.Empty<Layer>();
            CellDatas = Array.Empty<CellData>();
        }

        public void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(MapVersion);
            writer.WriteInt(Id);
            writer.WriteInt(RelativeId);
            writer.WriteSByte(MapType);
            writer.WriteInt(SubAreaId);

            writer.WriteInt(TopNeighbourId);
            writer.WriteInt(BottomNeighbourId);
            writer.WriteInt(LeftNeighbourId);
            writer.WriteInt(RightNeighbourId);

            writer.WriteInt(ShadowBonusOnEntities);

            writer.WriteByte((byte)BackgroundFixtures.Length);
            for (int i = 0; i < BackgroundFixtures.Length; i++)
                BackgroundFixtures[i].Serialize(writer);

            writer.WriteByte((byte)ForegroundFixtures.Length);
            for (int i = 0; i < ForegroundFixtures.Length; i++)
                ForegroundFixtures[i].Serialize(writer);

            writer.WriteInt(GroundCRC);
            writer.WriteInt(Checksum);

            writer.WriteByte((byte)Layers.Length);
            for (int i = 0; i < Layers.Length; i++)
                Layers[i].Serialize(writer);

            for (int i = 0; i < CellDatas.Length; i++)
                CellDatas[i].Serialize(writer);
        }

        private void Deserialize(BigEndianReader reader)
        {
            MapVersion = reader.ReadSByte();
            Id = reader.ReadInt();
            RelativeId = reader.ReadInt();
            MapType = reader.ReadSByte();
            SubAreaId = reader.ReadInt();

            TopNeighbourId = reader.ReadInt();
            BottomNeighbourId = reader.ReadInt();
            LeftNeighbourId = reader.ReadInt();
            RightNeighbourId = reader.ReadInt();

            ShadowBonusOnEntities = reader.ReadInt();

            BackgroundFixtures = new Fixture[reader.ReadByte()];
            for (int i = 0; i < BackgroundFixtures.Length; i++)
                BackgroundFixtures[i] = Fixture.FromRaw(reader);

            ForegroundFixtures = new Fixture[reader.ReadByte()];
            for (int i = 0; i < ForegroundFixtures.Length; i++)
                ForegroundFixtures[i] = Fixture.FromRaw(reader);

            GroundCRC = reader.ReadInt();
            Checksum = reader.ReadInt();

            Layers = new Layer[reader.ReadByte()];
            for (var i = 0; i < Layers.Length; i++)
                Layers[i] = Layer.FromRaw(reader);

            CellDatas = new CellData[cellsCount];
            for (short i = 0; i < CellDatas.Length; i++)
                CellDatas[i] = CellData.FromRaw(reader, i);
        }

        public static Map FromRaw(BigEndianReader reader)
        {
            var map = new Map();
            map.Deserialize(reader);

            return map;
        }
    }
}
