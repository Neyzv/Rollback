using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record GameActionFightPointsVariationMessage : AbstractGameActionMessage
	{
        public int targetId;
        public short delta;

        public new const int Id = 1030;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameActionFightPointsVariationMessage()
        {
        }
        public GameActionFightPointsVariationMessage(short actionId, int sourceId, int targetId, short delta) : base(actionId, sourceId)
        {
            this.targetId = targetId;
            this.delta = delta;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(targetId);
            writer.WriteShort(delta);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            targetId = reader.ReadInt();
            delta = reader.ReadShort();
		}
	}
}
