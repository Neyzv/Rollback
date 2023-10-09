using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record MapObstacleUpdateMessage : Message
	{
        public MapObstacle[] obstacles;

        public const int Id = 6051;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public MapObstacleUpdateMessage()
        {
        }
        public MapObstacleUpdateMessage(MapObstacle[] obstacles)
        {
            this.obstacles = obstacles;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)obstacles.Length);
            foreach (var entry in obstacles)
            {
                 entry.Serialize(writer);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            obstacles = new MapObstacle[limit];
            for (int i = 0; i < limit; i++)
            {
                 obstacles[i] = new MapObstacle();
                 obstacles[i].Deserialize(reader);
            }
		}
	}
}
