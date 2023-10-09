using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record CompassResetMessage : Message
	{
        public const int Id = 5584;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public CompassResetMessage()
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
