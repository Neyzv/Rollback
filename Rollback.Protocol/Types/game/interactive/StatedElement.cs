using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record StatedElement
    {
        public int elementId;
        public short elementCellId;
        public int elementState;
        public const short Id = 108;
        public virtual short TypeId
        {
            get { return Id; }
        }
        public StatedElement()
        {
        }
        public StatedElement(int elementId, short elementCellId, int elementState)
        {
            this.elementId = elementId;
            this.elementCellId = elementCellId;
            this.elementState = elementState;
        }
        public virtual void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(elementId);
            writer.WriteShort(elementCellId);
            writer.WriteInt(elementState);
        }
        public virtual void Deserialize(BigEndianReader reader)
        {
            elementId = reader.ReadInt();
            if (elementId < 0)
                throw new Exception("Forbidden value on elementId = " + elementId + ", it doesn't respect the following condition : elementId < 0");
            elementCellId = reader.ReadShort();
            if (elementCellId < 0 || elementCellId > 559)
                throw new Exception("Forbidden value on elementCellId = " + elementCellId + ", it doesn't respect the following condition : elementCellId < 0 || elementCellId > 559");
            elementState = reader.ReadInt();
            if (elementState < 0)
                throw new Exception("Forbidden value on elementState = " + elementState + ", it doesn't respect the following condition : elementState < 0");
        }
    }
}