using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record HouseGuildRightsViewMessage : Message
	{
        public const int Id = 5700;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public HouseGuildRightsViewMessage()
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
