using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record HelloGameMessage : Message
	{
        public const int Id = 101;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public HelloGameMessage()
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
