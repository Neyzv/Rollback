using Rollback.Common.IO.Binary;

namespace Rollback.Client.Dlm
{
    public sealed class Layer
    {
        #region Properties
        public int LayerId { get; set; }

        public Cell[] Cells { get; set; }
        #endregion

        public Layer() =>
            Cells = Array.Empty<Cell>();

        public void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(LayerId);

            writer.WriteShort((short)Cells.Length);
            for (int i = 0; i < Cells.Length; i++)
                Cells[i].Serialize(writer);
        }

        private void Deserialize(BigEndianReader reader)
        {
            LayerId = reader.ReadInt();

            Cells = new Cell[reader.ReadShort()];
            for (int i = 0; i < Cells.Length; i++)
                Cells[i] = Cell.FromRaw(reader);
        }

        public static Layer FromRaw(BigEndianReader reader)
        {
            var layer = new Layer();
            layer.Deserialize(reader);

            return layer;
        }
    }
}
