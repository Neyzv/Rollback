using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record GuildCreationStartedMessage : Message
	{
        public const int Id = 5920;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GuildCreationStartedMessage()
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
