using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record PaddockBuyRequestMessage : Message
	{
        public const int Id = 5951;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public PaddockBuyRequestMessage()
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
