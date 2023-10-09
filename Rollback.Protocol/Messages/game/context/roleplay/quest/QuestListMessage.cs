using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record QuestListMessage : Message
	{
        public short[] finishedQuestsIds;
        public short[] activeQuestsIds;

        public const int Id = 5626;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public QuestListMessage()
        {
        }
        public QuestListMessage(short[] finishedQuestsIds, short[] activeQuestsIds)
        {
            this.finishedQuestsIds = finishedQuestsIds;
            this.activeQuestsIds = activeQuestsIds;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)finishedQuestsIds.Length);
            foreach (var entry in finishedQuestsIds)
            {
                 writer.WriteShort(entry);
            }
            writer.WriteUShort((ushort)activeQuestsIds.Length);
            foreach (var entry in activeQuestsIds)
            {
                 writer.WriteShort(entry);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            finishedQuestsIds = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 finishedQuestsIds[i] = reader.ReadShort();
            }
            limit = reader.ReadUShort();
            activeQuestsIds = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 activeQuestsIds[i] = reader.ReadShort();
            }
		}
	}
}
