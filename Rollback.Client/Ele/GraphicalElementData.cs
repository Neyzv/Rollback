using Rollback.Client.Ele.GraphicalElements;
using Rollback.Common.IO.Binary;

namespace Rollback.Client.Ele
{
    public abstract class GraphicalElementData
    {
        public abstract GraphicalElementTypes Type { get; }

        public int Id { get; set; }

        public GraphicalElementData(int id) =>
            Id = id;

        protected abstract void InternalSerialize(BigEndianWriter writer);

        public void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(Id);
            writer.WriteSByte((sbyte)Type);
            InternalSerialize(writer);
        }

        protected abstract void Deserialize(BigEndianReader reader);

        public static GraphicalElementData FromRaw(BigEndianReader reader, int id)
        {
            GraphicalElementData element = (GraphicalElementTypes)reader.ReadSByte() switch
            {
                GraphicalElementTypes.Normal => new NormalGraphicalElementData(id),
                GraphicalElementTypes.BoudingBox => new BoudingBoxGraphicalElementData(id),
                GraphicalElementTypes.Animated => new AnimatedGraphicalElementData(id),
                GraphicalElementTypes.Entity => new EntityGraphicalElementData(id),
                GraphicalElementTypes.Particles => new ParticlesGraphicalElementData(id),
                _ => throw new Exception("Uknown element, please update your client datas...")
            };

            element.Deserialize(reader);

            return element;
        }
    }
}
