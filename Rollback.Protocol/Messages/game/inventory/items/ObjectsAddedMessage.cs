using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record ObjectsAddedMessage : Message
	{
        public ObjectItem[] @object;

        public const int Id = 6033;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ObjectsAddedMessage()
        {
        }
        public ObjectsAddedMessage(ObjectItem[] @object)
        {
            this.@object = @object;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)@object.Length);
            foreach (var entry in @object)
            {
                 entry.Serialize(writer);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            @object = new ObjectItem[limit];
            for (int i = 0; i < limit; i++)
            {
                 @object[i] = new ObjectItem();
                 @object[i].Deserialize(reader);
            }
		}
	}
}
