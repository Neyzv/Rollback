using Rollback.Common.IO.Binary;

namespace Rollback.Client.Dlm
{
    public sealed class CellData
    {
        #region Properties
        public short Id { get; }

        public short Floor { get; set; }

        public sbyte LosMov { get; set; }

        public sbyte Speed { get; set; }

        public byte MapChangeData { get; set; }

        public bool Los => (LosMov & 2) >> 1 == 1;

        public bool Mov => (LosMov & 1) == 1 && !NonWalkableDuringFight;

        public bool NonWalkableDuringFight => (LosMov & 3) >> 2 == 1;

        public bool Red => ((LosMov & 4) >> 3) == 1;

        public bool Blue => ((LosMov & 5) >> 4) == 1;
        #endregion

        public CellData(short id) =>
            Id = id;

        public void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte((sbyte)(Floor / 10));
            if (Floor is not -1280)
            {
                writer.WriteSByte(LosMov);
                writer.WriteSByte(Speed);
                writer.WriteByte(MapChangeData);
            }
        }

        private void Deserialize(BigEndianReader reader)
        {
            Floor = (short)(reader.ReadSByte() * 10);
            if (Floor is not -1280)
            {
                LosMov = reader.ReadSByte();
                Speed = reader.ReadSByte();
                MapChangeData = reader.ReadByte();
            }
        }

        public static CellData FromRaw(BigEndianReader reader, short id)
        {
            var cell = new CellData(id);
            cell.Deserialize(reader);

            return cell;
        }
    }
}
