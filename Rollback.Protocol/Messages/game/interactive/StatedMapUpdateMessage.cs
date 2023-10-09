using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record StatedMapUpdateMessage : Message
	{
        public StatedElement[] statedElements;

        public const int Id = 5716;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public StatedMapUpdateMessage()
        {
        }
        public StatedMapUpdateMessage(StatedElement[] statedElements)
        {
            this.statedElements = statedElements;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)statedElements.Length);
            foreach (var entry in statedElements)
            {
                 entry.Serialize(writer);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            statedElements = new StatedElement[limit];
            for (int i = 0; i < limit; i++)
            {
                 statedElements[i] = new StatedElement();
                 statedElements[i].Deserialize(reader);
            }
		}
	}
}
