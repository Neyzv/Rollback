using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record GameActionFightReflectSpellMessage : AbstractGameActionMessage
	{
        public int targetId;

        public new const int Id = 5531;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameActionFightReflectSpellMessage()
        {
        }
        public GameActionFightReflectSpellMessage(short actionId, int sourceId, int targetId) : base(actionId, sourceId)
        {
            this.targetId = targetId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(targetId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            targetId = reader.ReadInt();
		}
	}
}
