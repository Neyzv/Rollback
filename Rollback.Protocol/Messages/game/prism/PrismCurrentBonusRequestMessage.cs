using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record PrismCurrentBonusRequestMessage : Message
	{
        public const int Id = 5840;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public PrismCurrentBonusRequestMessage()
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
