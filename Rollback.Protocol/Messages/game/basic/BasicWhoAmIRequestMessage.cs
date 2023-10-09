using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record BasicWhoAmIRequestMessage : Message
	{
        public const int Id = 5664;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public BasicWhoAmIRequestMessage()
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
