using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record GuidedModeQuitRequestMessage : Message
	{
        public const int Id = 6092;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GuidedModeQuitRequestMessage()
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
