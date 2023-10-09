using Rollback.Common.IO.Binary;

namespace Rollback.Client.Dlm
{
    public sealed class Cell
    {
        #region Properties
        public short CellId { get; set; }

        public BasicElement[] Elements { get; set; }
        #endregion

        public Cell() =>
            Elements = Array.Empty<BasicElement>();

        public void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(CellId);

            writer.WriteShort((short)Elements.Length);
            for (int i = 0; i < Elements.Length; i++)
                Elements[i].Serialize(writer);
        }

        private void Deserialize(BigEndianReader reader)
        {
            CellId = reader.ReadShort();

            Elements = new BasicElement[reader.ReadShort()];
            for (int i = 0; i < Elements.Length; i++)
                Elements[i] = BasicElement.FromRaw(reader);
        }

        public static Cell FromRaw(BigEndianReader reader)
        {
            var cell = new Cell();
            cell.Deserialize(reader);

            return cell;
        }
    }
}
