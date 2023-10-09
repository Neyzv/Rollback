using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record NotificationListMessage : Message
	{
        public int[] flags;

        public const int Id = 6087;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public NotificationListMessage()
        {
        }
        public NotificationListMessage(int[] flags)
        {
            this.flags = flags;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)flags.Length);
            foreach (var entry in flags)
            {
                 writer.WriteInt(entry);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            flags = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 flags[i] = reader.ReadInt();
            }
		}
	}
}
