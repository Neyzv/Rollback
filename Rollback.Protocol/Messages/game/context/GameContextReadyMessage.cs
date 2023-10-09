using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record GameContextReadyMessage : Message
	{
        public const int Id = 6071;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameContextReadyMessage()
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
