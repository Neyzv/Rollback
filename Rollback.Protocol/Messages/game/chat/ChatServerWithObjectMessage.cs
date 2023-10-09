using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record ChatServerWithObjectMessage : ChatServerMessage
	{
        public ObjectItem[] objects;

        public new const int Id = 883;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ChatServerWithObjectMessage()
        {
        }
        public ChatServerWithObjectMessage(sbyte channel, string content, int timestamp, string fingerprint, int senderId, string senderName, ObjectItem[] objects) : base(channel, content, timestamp, fingerprint, senderId, senderName)
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
