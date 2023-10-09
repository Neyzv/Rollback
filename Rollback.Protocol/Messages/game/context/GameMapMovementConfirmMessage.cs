using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record GameMapMovementConfirmMessage : Message
	{
        public const int Id = 952;
        public override uint MessageId
        {
        	get { return 952; }
        }
        public GameMapMovementConfirmMessage()
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
