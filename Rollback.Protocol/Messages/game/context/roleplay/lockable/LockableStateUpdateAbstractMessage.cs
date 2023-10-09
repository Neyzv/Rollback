using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record LockableStateUpdateAbstractMessage : Message
	{
        public bool locked;

        public const int Id = 5671;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public LockableStateUpdateAbstractMessage()
        {
        }
        public LockableStateUpdateAbstractMessage(bool locked)
        {
            this.locked = locked;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteBoolean(locked);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            locked = reader.ReadBoolean();
		}
	}
}
