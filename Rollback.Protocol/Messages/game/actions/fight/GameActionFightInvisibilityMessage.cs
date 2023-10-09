using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record GameActionFightInvisibilityMessage : AbstractGameActionMessage
	{
        public int targetId;
        public sbyte state;

        public new const int Id = 5821;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameActionFightInvisibilityMessage()
        {
        }
        public GameActionFightInvisibilityMessage(short actionId, int sourceId, int targetId, sbyte state) : base(actionId, sourceId)
        {
            this.targetId = targetId;
            this.state = state;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(targetId);
            writer.WriteSByte(state);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            targetId = reader.ReadInt();
            state = reader.ReadSByte();
		}
	}
}
