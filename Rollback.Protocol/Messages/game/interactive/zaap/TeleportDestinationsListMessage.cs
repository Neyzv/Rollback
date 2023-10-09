using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record TeleportDestinationsListMessage : Message
	{
        public sbyte teleporterType;
        public int[] mapIds;
        public short[] subareaIds;
        public short[] costs;

        public const int Id = 5960;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public TeleportDestinationsListMessage()
        {
        }
        public TeleportDestinationsListMessage(sbyte teleporterType, int[] mapIds, short[] subareaIds, short[] costs)
        {
            this.teleporterType = teleporterType;
            this.mapIds = mapIds;
            this.subareaIds = subareaIds;
            this.costs = costs;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(teleporterType);
            writer.WriteUShort((ushort)mapIds.Length);
            foreach (var entry in mapIds)
            {
                 writer.WriteInt(entry);
            }
            writer.WriteUShort((ushort)subareaIds.Length);
            foreach (var entry in subareaIds)
            {
                 writer.WriteShort(entry);
            }
            writer.WriteUShort((ushort)costs.Length);
            foreach (var entry in costs)
            {
                 writer.WriteShort(entry);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            teleporterType = reader.ReadSByte();
            if (teleporterType < 0)
                throw new Exception("Forbidden value on teleporterType = " + teleporterType + ", it doesn't respect the following condition : teleporterType < 0");
            var limit = reader.ReadUShort();
            mapIds = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 mapIds[i] = reader.ReadInt();
            }
            limit = reader.ReadUShort();
            subareaIds = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 subareaIds[i] = reader.ReadShort();
            }
            limit = reader.ReadUShort();
            costs = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 costs[i] = reader.ReadShort();
            }
		}
	}
}
