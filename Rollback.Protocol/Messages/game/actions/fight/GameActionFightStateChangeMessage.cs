using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record GameActionFightStateChangeMessage : AbstractGameActionMessage
	{
        public int targetId;
        public short stateId;
        public bool active;

        public new const int Id = 5569;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameActionFightStateChangeMessage()
        {
        }
        public GameActionFightStateChangeMessage(short actionId, int sourceId, int targetId, short stateId, bool active) : base(actionId, sourceId)
        {
            this.targetId = targetId;
            this.stateId = stateId;
            this.active = active;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(targetId);
            writer.WriteShort(stateId);
            writer.WriteBoolean(active);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            targetId = reader.ReadInt();
            stateId = reader.ReadShort();
            active = reader.ReadBoolean();
		}
	}
}
