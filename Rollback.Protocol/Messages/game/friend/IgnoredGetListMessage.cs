using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record IgnoredGetListMessage : Message
	{
        public const int Id = 5676;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public IgnoredGetListMessage()
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
