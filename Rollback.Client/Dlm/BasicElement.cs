using Rollback.Client.Dlm.Elements;
using Rollback.Common.IO.Binary;

namespace Rollback.Client.Dlm
{
    public abstract class BasicElement
    {
        public abstract ElementTypes Type { get; }

        protected abstract void InternalSerialize(BigEndianWriter writer);
        public void Serialize(BigEndianWriter writer)
        {
            writer.WriteByte((byte)Type);
            InternalSerialize(writer);
        }

        protected abstract void Deserialize(BigEndianReader reader);

        public static BasicElement FromRaw(BigEndianReader reader)
        {
            BasicElement element = (ElementTypes)reader.ReadByte() switch
            {
                ElementTypes.Graphical => new GraphicalElement(),
                ElementTypes.Sound => new SoundElement(),
                _ => throw new Exception("Uknown element, please update datas...")
            };

            element.Deserialize(reader);

            return element;
        }
    }
}
