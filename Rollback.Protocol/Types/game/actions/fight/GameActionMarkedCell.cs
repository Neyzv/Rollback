using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record GameActionMarkedCell
	{ 
		public short cellId;
        public sbyte zoneSize;
        public int cellColor;
		public const short Id = 85;
		public virtual short TypeId
		{
			get { return Id; }
		}
		public GameActionMarkedCell()
		{
		}
		public GameActionMarkedCell(short cellId, sbyte zoneSize, int cellColor)
		{
			this.cellId = cellId;
			this.zoneSize = zoneSize;
			this.cellColor = cellColor;
		}
		public virtual void Serialize(BigEndianWriter writer)
		{
            writer.WriteShort(cellId);
			writer.WriteSByte(zoneSize);
			writer.WriteInt(cellColor);
		}
		public virtual void Deserialize(BigEndianReader reader)
		{
            cellId = reader.ReadShort();
			if (cellId < 0 || cellId > 559)
				throw new Exception("Forbidden value on cellId = " + cellId + ", it doesn't respect the following condition : cellId < 0 || cellId > 559");
			zoneSize = reader.ReadSByte();
			cellColor = reader.ReadInt();
		}
	}
}