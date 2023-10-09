using System.Drawing;
using Rollback.Common.IO.Binary;

namespace Rollback.Client.Dlm
{
    public sealed class Fixture
    {
        #region Properties
        public int FixtureId { get; set; }

        public Point Offset { get; set; }

        public short Rotation { get; set; }

        public Point Scale { get; set; }

        public Color ColorMultiplier { get; set; }

        public int Hue =>
            ColorMultiplier.R << 16 | ColorMultiplier.G << 8 | ColorMultiplier.B;
        #endregion

        public void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(FixtureId);

            writer.WriteShort((short)Offset.X);
            writer.WriteShort((short)Offset.Y);

            writer.WriteShort(Rotation);

            writer.WriteShort((short)Scale.X);
            writer.WriteShort((short)Scale.Y);

            writer.WriteByte(ColorMultiplier.R);
            writer.WriteByte(ColorMultiplier.G);
            writer.WriteByte(ColorMultiplier.B);
            writer.WriteByte(ColorMultiplier.A);
        }

        private void Deserialize(BigEndianReader reader)
        {
            FixtureId = reader.ReadInt();
            Offset = new(reader.ReadShort(), reader.ReadShort());
            Rotation = reader.ReadShort();
            Scale = new(reader.ReadShort(), reader.ReadShort());

            var red = reader.ReadByte();
            var green = reader.ReadByte();
            var blue = reader.ReadByte();
            var alpha = reader.ReadByte();
            ColorMultiplier = Color.FromArgb(alpha, red, green, blue);
        }

        public static Fixture FromRaw(BigEndianReader reader)
        {
            var fixture = new Fixture();
            fixture.Deserialize(reader);

            return fixture;
        }
    }
}
