using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record GameMapMovementRequestMessage : Message
	{
        public int mapId;
        public short[] keyMovements;

        public const int Id = 950;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameMapMovementRequestMessage()
        {
        }
        public GameMapMovementRequestMessage(int mapId, short[] keyMovements)
        {
            this.mapId = mapId;
            this.keyMovements = keyMovements;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(mapId);
            writer.WriteUShort((ushort)keyMovements.Length);
            foreach (var entry in keyMovements)
            {
                 writer.WriteShort(entry);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            mapId = reader.ReadInt();
            if (mapId < 0)
                throw new Exception("Forbidden value on mapId = " + mapId + ", it doesn't respect the following condition : mapId < 0");
            var limit = reader.ReadUShort();
            keyMovements = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 keyMovements[i] = reader.ReadShort();
            }
		}
	}
}
