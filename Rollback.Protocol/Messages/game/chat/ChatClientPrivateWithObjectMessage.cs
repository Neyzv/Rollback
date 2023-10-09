using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record ChatClientPrivateWithObjectMessage : ChatClientPrivateMessage
	{
        public ObjectItem[] objects;

        public new const int Id = 852;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ChatClientPrivateWithObjectMessage()
        {
        }
        public ChatClientPrivateWithObjectMessage(string content, string receiver, ObjectItem[] objects) : base(content, receiver)
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
