using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record GameActionNoopMessage : Message
	{
        public const int Id = 1002;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameActionNoopMessage()
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
