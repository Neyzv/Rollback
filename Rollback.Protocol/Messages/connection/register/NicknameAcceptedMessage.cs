using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record NicknameAcceptedMessage : Message
	{
        public const int Id = 5641;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public NicknameAcceptedMessage()
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
