using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record GameContextCreateErrorMessage : Message
	{
        public const int Id = 6024;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameContextCreateErrorMessage()
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
