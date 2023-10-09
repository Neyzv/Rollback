using System.Drawing;
using Rollback.Common.IO.Binary;

namespace Rollback.Client.Ele.GraphicalElements
{
    public class NormalGraphicalElementData : GraphicalElementData
    {
        public override GraphicalElementTypes Type =>
            GraphicalElementTypes.Normal;

        #region Properties
        public int GfxId { get; set; }

        public sbyte Height { get; set; }

        public bool HorizontalSymmetry { get; set; }

        public Point Origin { get; set; }

        public Point Size { get; set; }
        #endregion

        public NormalGraphicalElementData(int id) : base(id) { }

        protected override void InternalSerialize(BigEndianWriter writer)
        {
            writer.WriteInt(GfxId);
            writer.WriteSByte(Height);
            writer.WriteBoolean(HorizontalSymmetry);

            writer.WriteShort((short)Origin.X);
            writer.WriteShort((short)Origin.Y);

            writer.WriteShort((short)Size.X);
            writer.WriteShort((short)Size.Y);
        }

        protected override void Deserialize(BigEndianReader reader)
        {
            GfxId = reader.ReadInt();
            Height = reader.ReadSByte();
            HorizontalSymmetry = reader.ReadBoolean();
            Origin = new Point(reader.ReadShort(), reader.ReadShort());
            Size = new Point(reader.ReadShort(), reader.ReadShort());
        }
    }
}
