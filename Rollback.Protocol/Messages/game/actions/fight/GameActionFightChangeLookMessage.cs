using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record GameActionFightChangeLookMessage : AbstractGameActionMessage
	{
        public int targetId;
        public EntityLook entityLook;

        public new const int Id = 5532;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameActionFightChangeLookMessage()
        {
        }
        public GameActionFightChangeLookMessage(short actionId, int sourceId, int targetId, EntityLook entityLook) : base(actionId, sourceId)
        {
            this.targetId = targetId;
            this.entityLook = entityLook;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(targetId);
            entityLook.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            targetId = reader.ReadInt();
            entityLook = new EntityLook();
            entityLook.Deserialize(reader);
		}
	}
}
