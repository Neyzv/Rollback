using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record AbstractGameActionWithAckMessage : AbstractGameActionMessage
	{
        public short waitAckId;

        public new const int Id = 1001;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public AbstractGameActionWithAckMessage()
        {
        }
        public AbstractGameActionWithAckMessage(short actionId, int sourceId, short waitAckId) : base(actionId, sourceId)
        {
            this.waitAckId = waitAckId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(waitAckId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            waitAckId = reader.ReadShort();
		}
	}
}
