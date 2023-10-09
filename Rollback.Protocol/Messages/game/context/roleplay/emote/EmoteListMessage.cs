using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record EmoteListMessage : Message
	{
        public sbyte[] emoteIds;

        public const int Id = 5689;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public EmoteListMessage()
        {
        }
        public EmoteListMessage(sbyte[] emoteIds)
        {
            this.emoteIds = emoteIds;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)emoteIds.Length);
            foreach (var entry in emoteIds)
            {
                 writer.WriteSByte(entry);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            emoteIds = new sbyte[limit];
            for (int i = 0; i < limit; i++)
            {
                 emoteIds[i] = reader.ReadSByte();
            }
		}
	}
}
