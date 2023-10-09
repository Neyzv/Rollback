using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record PrismInfoCloseMessage : Message
	{
        public const int Id = 5853;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public PrismInfoCloseMessage()
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
