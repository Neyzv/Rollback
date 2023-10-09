using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record MapRunningFightListRequestMessage : Message
	{
        public const int Id = 5742;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public MapRunningFightListRequestMessage()
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
