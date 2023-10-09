using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record ChatClientMultiWithObjectMessage : ChatClientMultiMessage
	{
        public ObjectItem[] objects;

        public new const int Id = 862;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ChatClientMultiWithObjectMessage()
        {
        }
        public ChatClientMultiWithObjectMessage(string content, sbyte channel, ObjectItem[] objects) : base(content, channel)
        {
            this.objects = objects;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUShort((ushort)objects.Length);
            foreach (var entry in objects)
            {
                 entry.Serialize(writer);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            objects = new ObjectItem[limit];
            for (int i = 0; i < limit; i++)
            {
                 objects[i] = new ObjectItem();
                 objects[i].Deserialize(reader);
            }
		}
	}
}
