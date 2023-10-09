using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record GameContextDestroyMessage : Message
	{
        public const int Id = 201;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameContextDestroyMessage()
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
