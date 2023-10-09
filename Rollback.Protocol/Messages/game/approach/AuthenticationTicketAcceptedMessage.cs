using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record AuthenticationTicketAcceptedMessage : Message
	{
        public const int Id = 111;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public AuthenticationTicketAcceptedMessage()
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
