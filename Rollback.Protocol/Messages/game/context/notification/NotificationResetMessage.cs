using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record NotificationResetMessage : Message
	{
        public const int Id = 6089;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public NotificationResetMessage()
        {
        }
        public override void Serialize(BigEndianWriter writer)
        {
        }
        public override void Deserialize(BigEndianReader reader)
        {
		}
	}
}
