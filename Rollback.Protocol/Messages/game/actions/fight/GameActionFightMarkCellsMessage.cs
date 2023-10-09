using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record GameActionFightMarkCellsMessage : AbstractGameActionMessage
	{
        public short markId;
        public sbyte markType;
        public GameActionMarkedCell[] cells;

        public new const int Id = 5540;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameActionFightMarkCellsMessage()
        {
        }
        public GameActionFightMarkCellsMessage(short actionId, int sourceId, short markId, sbyte markType, GameActionMarkedCell[] cells) : base(actionId, sourceId)
        {
            this.markId = markId;
            this.markType = markType;
            this.cells = cells;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(markId);
            writer.WriteSByte(markType);
            writer.WriteUShort((ushort)cells.Length);
            foreach (var entry in cells)
            {
                 entry.Serialize(writer);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            markId = reader.ReadShort();
            markType = reader.ReadSByte();
            var limit = reader.ReadUShort();
            cells = new GameActionMarkedCell[limit];
            for (int i = 0; i < limit; i++)
            {
                 cells[i] = new GameActionMarkedCell();
                 cells[i].Deserialize(reader);
            }
		}
	}
}
