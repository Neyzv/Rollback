using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record EntityDispositionInformations
    {
        public short cellId;
        public sbyte direction;
        public const short Id = 60;
        public virtual short TypeId
        {
            get { return Id; }
        }
        public EntityDispositionInformations()
        {
        }
        public EntityDispositionInformations(short cellId, sbyte direction)
        {
            this.cellId = cellId;
            this.direction = direction;
        }
        public virtual void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(cellId);
            writer.WriteSByte(direction);
        }
        public virtual void Deserialize(BigEndianReader reader)
        {
            cellId = reader.ReadShort();
            if (cellId < -1 || cellId > 559)
                throw new Exception("Forbidden value on cellId = " + cellId + ", it doesn't respect the following condition : cellId < -1 || cellId > 559");
            direction = reader.ReadSByte();
            if (direction < 0)
                throw new Exception("Forbidden value on direction = " + direction + ", it doesn't respect the following condition : direction < 0");
        }
    }
}
