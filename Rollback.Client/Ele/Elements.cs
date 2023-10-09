using Rollback.Common.IO.Binary;

namespace Rollback.Client.Ele
{
    public sealed class Elements
    {
        #region Properties
        public sbyte Version { get; set; }

        public Dictionary<int, GraphicalElementData> GraphicalDatas { get; set; }
        #endregion

        public Elements() =>
            GraphicalDatas = new();

        public void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(Version);

            writer.WriteInt(GraphicalDatas.Count);
            foreach (var data in GraphicalDatas.Values)
                data.Serialize(writer);
        }

        private void Deserialize(BigEndianReader reader)
        {
            Version = reader.ReadSByte();

            var count = reader.ReadInt();
            for (var i = 0; i < count; i++)
            {
                var id = reader.ReadInt();
                GraphicalDatas[id] = GraphicalElementData.FromRaw(reader, id);
            }
        }

        public static Elements FromRaw(BigEndianReader reader)
        {
            var elements = new Elements();
            elements.Deserialize(reader);

            return elements;
        }
    }
}
