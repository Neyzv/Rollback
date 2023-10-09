using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record GameActionAcknowledgementMessage : Message
	{
        public bool valid;
        public int actionId;

        public const int Id = 957;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameActionAcknowledgementMessage()
        {
        }
        public GameActionAcknowledgementMessage(bool valid, int actionId)
        {
            this.valid = valid;
            this.actionId = actionId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteBoolean(valid);
            writer.WriteInt(actionId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            valid = reader.ReadBoolean();
            actionId = reader.ReadInt();
		}
	}
}
