using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record BasicLatencyStatsRequestMessage : Message
	{
        public const int Id = 5816;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public BasicLatencyStatsRequestMessage()
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
