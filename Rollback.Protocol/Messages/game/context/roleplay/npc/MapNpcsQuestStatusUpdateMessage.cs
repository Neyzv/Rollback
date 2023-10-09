using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record MapNpcsQuestStatusUpdateMessage : Message
	{
        public int mapId;
        public int[] npcsIdsCanGiveQuest;
        public int[] npcsIdsCannotGiveQuest;

        public const int Id = 5642;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public MapNpcsQuestStatusUpdateMessage()
        {
        }
        public MapNpcsQuestStatusUpdateMessage(int mapId, int[] npcsIdsCanGiveQuest, int[] npcsIdsCannotGiveQuest)
        {
            this.mapId = mapId;
            this.npcsIdsCanGiveQuest = npcsIdsCanGiveQuest;
            this.npcsIdsCannotGiveQuest = npcsIdsCannotGiveQuest;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(mapId);
            writer.WriteUShort((ushort)npcsIdsCanGiveQuest.Length);
            foreach (var entry in npcsIdsCanGiveQuest)
            {
                 writer.WriteInt(entry);
            }
            writer.WriteUShort((ushort)npcsIdsCannotGiveQuest.Length);
            foreach (var entry in npcsIdsCannotGiveQuest)
            {
                 writer.WriteInt(entry);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            mapId = reader.ReadInt();
            var limit = reader.ReadUShort();
            npcsIdsCanGiveQuest = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 npcsIdsCanGiveQuest[i] = reader.ReadInt();
            }
            limit = reader.ReadUShort();
            npcsIdsCannotGiveQuest = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 npcsIdsCannotGiveQuest[i] = reader.ReadInt();
            }
		}
	}
}
