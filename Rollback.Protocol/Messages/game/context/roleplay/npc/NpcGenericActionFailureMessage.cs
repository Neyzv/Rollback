using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record NpcGenericActionFailureMessage : Message
	{
        public const int Id = 5900;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public NpcGenericActionFailureMessage()
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
