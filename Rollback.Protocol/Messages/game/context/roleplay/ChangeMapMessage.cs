using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record ChangeMapMessage : Message
	{
        public int mapId;

        public const int Id = 221;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ChangeMapMessage()
        {
        }
        public ChangeMapMessage(int mapId)
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
