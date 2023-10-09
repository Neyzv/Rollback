using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record AlignmentSubAreaUpdateMessage : Message
	{
        public short subAreaId;
        public sbyte side;
        public bool quiet;

        public const int Id = 6057;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public AlignmentSubAreaUpdateMessage()
        {
        }
        public AlignmentSubAreaUpdateMessage(short subAreaId, sbyte side, bool quiet)
        {
            this.subAreaId = subAreaId;
            this.side = side;
            this.quiet = quiet;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(subAreaId);
            writer.WriteSByte(side);
            writer.WriteBoolean(quiet);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            subAreaId = reader.ReadShort();
            if (subAreaId < 0)
                throw new Exception("Forbidden value on subAreaId = " + subAreaId + ", it doesn't respect the following condition : subAreaId < 0");
            side = reader.ReadSByte();
            quiet = reader.ReadBoolean();
		}
	}
}
