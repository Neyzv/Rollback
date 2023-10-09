using System.Drawing;
using Rollback.Common.IO.Binary;

namespace Rollback.Client.Dlm.Elements
{
    public sealed class GraphicalElement : BasicElement
    {
        #region Properties
        public override ElementTypes Type =>
            ElementTypes.Graphical;

        public int ElementId { get; set; }

        public ColorMultiplicator? Hue { get; set; }

        public ColorMultiplicator? Shadow { get; set; }

        public ColorMultiplicator? FinalTeint { get; set; }

        public Point Offset { get; set; }

        public sbyte Altitude { get; set; }

        public uint Identifier { get; set; }
        #endregion

        protected override void InternalSerialize(BigEndianWriter writer)
        {
            writer.WriteInt(ElementId);

            writer.WriteByte((byte)Hue!.Red);
            writer.WriteByte((byte)Hue.Green);
            writer.WriteByte((byte)Hue.Blue);

            writer.WriteByte((byte)Shadow!.Red);
            writer.WriteByte((byte)Shadow.Green);
            writer.WriteByte((byte)Shadow.Blue);

            writer.WriteByte((byte)Offset.X);
            writer.WriteByte((byte)Offset.Y);

            writer.WriteSByte(Altitude);

            writer.WriteUInt(Identifier);
        }

        protected override void Deserialize(BigEndianReader reader)
        {
            ElementId = reader.ReadInt();
            Hue = new ColorMultiplicator(reader.ReadByte(), reader.ReadByte(), reader.ReadByte());
            Shadow = new ColorMultiplicator(reader.ReadByte(), reader.ReadByte(), reader.ReadByte());
            Offset = new Point(reader.ReadByte(), reader.ReadByte());
            Altitude = reader.ReadSByte();
            Identifier = reader.ReadUInt();

            CalculateFinalTeint();
        }

        public void CalculateFinalTeint()
        {
            int r = Hue!.Red + Shadow!.Red;
            int g = Hue.Green + Shadow.Green;
            int b = Hue.Blue + Shadow.Blue;
            r = ColorMultiplicator.Clamp((r + 128) * 2, 0, 512);
            g = ColorMultiplicator.Clamp((g + 128) * 2, 0, 512);
            b = ColorMultiplicator.Clamp((b + 128) * 2, 0, 512);
            FinalTeint = new ColorMultiplicator(r, g, b, true);
        }
    }
}
