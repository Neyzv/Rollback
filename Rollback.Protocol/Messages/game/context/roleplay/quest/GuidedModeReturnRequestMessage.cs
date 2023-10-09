using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record GuidedModeReturnRequestMessage : Message
	{
        public const int Id = 6088;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GuidedModeReturnRequestMessage()
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
