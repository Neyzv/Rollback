using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record InteractiveMapUpdateMessage : Message
	{
        public InteractiveElement[] interactiveElements;

        public const int Id = 5002;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public InteractiveMapUpdateMessage()
        {
        }
        public InteractiveMapUpdateMessage(InteractiveElement[] interactiveElements)
        {
            this.interactiveElements = interactiveElements;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)interactiveElements.Length);
            foreach (var entry in interactiveElements)
            {
                 entry.Serialize(writer);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            interactiveElements = new InteractiveElement[limit];
            for (int i = 0; i < limit; i++)
            {
                 interactiveElements[i] = new InteractiveElement();
                 interactiveElements[i].Deserialize(reader);
            }
		}
	}
}
