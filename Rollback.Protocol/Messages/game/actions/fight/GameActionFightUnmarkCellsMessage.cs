using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record GameActionFightUnmarkCellsMessage : AbstractGameActionMessage
	{
        public short markId;

        public new const int Id = 5570;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameActionFightUnmarkCellsMessage()
        {
        }
        public GameActionFightUnmarkCellsMessage(short actionId, int sourceId, short markId) : base(actionId, sourceId)
        {
            this.markId = markId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(markId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            markId = reader.ReadShort();
		}
	}
}
