using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record ExchangeLeaveMessage : LeaveDialogMessage
	{
        public bool success;

        public new const int Id = 5628;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeLeaveMessage()
        {
        }
        public ExchangeLeaveMessage(bool success)
        {
            this.success = success;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteBoolean(success);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            success = reader.ReadBoolean();
		}
	}
}
