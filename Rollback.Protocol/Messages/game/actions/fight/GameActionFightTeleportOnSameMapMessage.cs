using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record GameActionFightTeleportOnSameMapMessage : AbstractGameActionMessage
	{
        public int targetId;
        public short cellId;

        public new const int Id = 5528;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameActionFightTeleportOnSameMapMessage()
        {
        }
        public GameActionFightTeleportOnSameMapMessage(short actionId, int sourceId, int targetId, short cellId) : base(actionId, sourceId)
        {
            this.targetId = targetId;
            this.cellId = cellId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(targetId);
            writer.WriteShort(cellId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            targetId = reader.ReadInt();
            cellId = reader.ReadShort();
            if (cellId < -1 || cellId > 559)
                throw new Exception("Forbidden value on cellId = " + cellId + ", it doesn't respect the following condition : cellId < -1 || cellId > 559");
		}
	}
}
