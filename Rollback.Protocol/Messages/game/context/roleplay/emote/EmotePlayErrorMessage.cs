using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record EmotePlayErrorMessage : Message
	{
        public const int Id = 5688;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public EmotePlayErrorMessage()
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
