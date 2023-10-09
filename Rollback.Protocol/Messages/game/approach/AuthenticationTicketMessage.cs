using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record AuthenticationTicketMessage : Message
	{
        public string ticket;
        public string lang;

        public const int Id = 110;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public AuthenticationTicketMessage()
        {
        }
        public AuthenticationTicketMessage(string ticket, string lang)
        {
            this.ticket = ticket;
            this.lang = lang;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteString(ticket);
            writer.WriteString(lang);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            ticket = reader.ReadString();
            lang = reader.ReadString();
		}
	}
}
