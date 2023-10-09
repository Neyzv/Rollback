using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record InteractiveElementUpdatedMessage : Message
	{
        public InteractiveElement interactiveElement;

        public const int Id = 5708;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public InteractiveElementUpdatedMessage()
        {
        }
        public InteractiveElementUpdatedMessage(InteractiveElement interactiveElement)
        {
            this.interactiveElement = interactiveElement;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            interactiveElement.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            interactiveElement = new InteractiveElement();
            interactiveElement.Deserialize(reader);
		}
	}
}
