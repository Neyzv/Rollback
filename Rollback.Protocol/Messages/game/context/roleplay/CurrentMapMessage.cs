using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record CurrentMapMessage : Message
	{
        public int mapId;

        public const int Id = 220;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public CurrentMapMessage()
        {
        }
        public CurrentMapMessage(int mapId)
        {
            this.mapId = mapId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(mapId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            mapId = reader.ReadInt();
            if (mapId < 0)
                throw new Exception("Forbidden value on mapId = " + mapId + ", it doesn't respect the following condition : mapId < 0");
		}
	}
}
