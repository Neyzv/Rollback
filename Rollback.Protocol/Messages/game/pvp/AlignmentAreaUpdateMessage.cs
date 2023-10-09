using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record AlignmentAreaUpdateMessage : Message
	{
        public short areaId;
        public sbyte side;

        public const int Id = 6060;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public AlignmentAreaUpdateMessage()
        {
        }
        public AlignmentAreaUpdateMessage(short areaId, sbyte side)
        {
            this.areaId = areaId;
            this.side = side;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(areaId);
            writer.WriteSByte(side);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            areaId = reader.ReadShort();
            if (areaId < 0)
                throw new Exception("Forbidden value on areaId = " + areaId + ", it doesn't respect the following condition : areaId < 0");
            side = reader.ReadSByte();
		}
	}
}
