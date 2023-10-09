using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record GameEntityDispositionErrorMessage : Message
	{
        public const int Id = 5695;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameEntityDispositionErrorMessage()
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
