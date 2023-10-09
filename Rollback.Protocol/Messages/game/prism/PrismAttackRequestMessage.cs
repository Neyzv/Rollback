using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record PrismAttackRequestMessage : Message
	{
        public const int Id = 6042;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public PrismAttackRequestMessage()
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
