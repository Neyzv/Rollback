using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record SpellUpgradeFailureMessage : Message
	{
        public const int Id = 1202;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public SpellUpgradeFailureMessage()
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
