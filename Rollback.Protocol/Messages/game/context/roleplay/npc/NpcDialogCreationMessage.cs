using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record NpcDialogCreationMessage : Message
	{
        public int mapId;
        public int npcId;

        public const int Id = 5618;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public NpcDialogCreationMessage()
        {
        }
        public NpcDialogCreationMessage(int mapId, int npcId)
        {
            this.mapId = mapId;
            this.npcId = npcId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(mapId);
            writer.WriteInt(npcId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            mapId = reader.ReadInt();
            npcId = reader.ReadInt();
		}
	}
}
