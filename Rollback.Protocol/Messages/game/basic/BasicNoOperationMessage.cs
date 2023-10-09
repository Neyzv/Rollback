using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record BasicNoOperationMessage : Message
	{
        public const int Id = 176;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public BasicNoOperationMessage()
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
