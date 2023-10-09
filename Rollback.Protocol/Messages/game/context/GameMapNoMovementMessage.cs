using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record GameMapNoMovementMessage : Message
	{
        public const int Id = 954;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameMapNoMovementMessage()
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
