using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record GameContextCreateRequestMessage : Message
	{
        public const int Id = 250;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameContextCreateRequestMessage()
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
