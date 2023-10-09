using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record CharacterSelectedForceReadyMessage : Message
	{
        public const int Id = 6072;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public CharacterSelectedForceReadyMessage()
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
