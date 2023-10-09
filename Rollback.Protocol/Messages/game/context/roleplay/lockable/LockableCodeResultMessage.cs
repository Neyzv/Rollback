using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record LockableCodeResultMessage : Message
	{
        public bool success;

        public const int Id = 5672;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public LockableCodeResultMessage()
        {
        }
        public LockableCodeResultMessage(bool success)
        {
            this.success = success;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteBoolean(success);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            success = reader.ReadBoolean();
		}
	}
}
