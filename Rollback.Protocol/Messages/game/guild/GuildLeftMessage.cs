using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record GuildLeftMessage : Message
	{
        public const int Id = 5562;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GuildLeftMessage()
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
