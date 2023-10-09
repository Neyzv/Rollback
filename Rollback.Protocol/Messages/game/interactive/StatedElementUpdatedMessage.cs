using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record StatedElementUpdatedMessage : Message
	{
        public StatedElement statedElement;

        public const int Id = 5709;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public StatedElementUpdatedMessage()
        {
        }
        public StatedElementUpdatedMessage(StatedElement statedElement)
        {
            this.statedElement = statedElement;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            statedElement.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            statedElement = new StatedElement();
            statedElement.Deserialize(reader);
		}
	}
}
