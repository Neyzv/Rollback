using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record AlignmentRankUpdateMessage : Message
	{
        public sbyte alignmentRank;
        public bool verbose;

        public const int Id = 6058;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public AlignmentRankUpdateMessage()
        {
        }
        public AlignmentRankUpdateMessage(sbyte alignmentRank, bool verbose)
        {
            this.alignmentRank = alignmentRank;
            this.verbose = verbose;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(alignmentRank);
            writer.WriteBoolean(verbose);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            alignmentRank = reader.ReadSByte();
            if (alignmentRank < 0)
                throw new Exception("Forbidden value on alignmentRank = " + alignmentRank + ", it doesn't respect the following condition : alignmentRank < 0");
            verbose = reader.ReadBoolean();
		}
	}
}
