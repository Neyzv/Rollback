using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record MapObstacle
    {
        public short obstacleCellId;
        public sbyte state;
        public const short Id = 200;
        public virtual short TypeId
        {
            get { return Id; }
        }
        public MapObstacle()
        {
        }
        public MapObstacle(short obstacleCellId, sbyte state)
        {
            this.obstacleCellId = obstacleCellId;
            this.state = state;
        }
        public virtual void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(obstacleCellId);
            writer.WriteSByte(state);
        }
        public virtual void Deserialize(BigEndianReader reader)
        {
            obstacleCellId = reader.ReadShort();
            if (obstacleCellId < 0 || obstacleCellId > 559)
                throw new Exception("Forbidden value on obstacleCellId = " + obstacleCellId + ", it doesn't respect the following condition : obstacleCellId < 0 || obstacleCellId > 559");
            state = reader.ReadSByte();
            if (state < 0)
                throw new Exception("Forbidden value on state = " + state + ", it doesn't respect the following condition : state < 0");
        }
    }
}