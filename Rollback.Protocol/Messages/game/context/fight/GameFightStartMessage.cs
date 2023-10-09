using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record GameFightStartMessage : Message
	{

        public const int Id = 712;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameFightStartMessage()
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
