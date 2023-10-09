using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record AuthenticationTicketRefusedMessage : Message
	{
        public const int Id = 112;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public AuthenticationTicketRefusedMessage()
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
