using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record ZaapListMessage : TeleportDestinationsListMessage
	{
        public int spawnMapId;

        public new const int Id = 1604;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ZaapListMessage()
        {
        }
        public ZaapListMessage(sbyte teleporterType, int[] mapIds, short[] subareaIds, short[] costs, int spawnMapId) : base(teleporterType, mapIds, subareaIds, costs)
        {
            this.spawnMapId = spawnMapId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(spawnMapId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            spawnMapId = reader.ReadInt();
            if (spawnMapId < 0)
                throw new Exception("Forbidden value on spawnMapId = " + spawnMapId + ", it doesn't respect the following condition : spawnMapId < 0");
		}
	}
}
