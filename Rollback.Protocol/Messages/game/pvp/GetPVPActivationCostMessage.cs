using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record GetPVPActivationCostMessage : Message
	{
        public const int Id = 1811;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GetPVPActivationCostMessage()
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
