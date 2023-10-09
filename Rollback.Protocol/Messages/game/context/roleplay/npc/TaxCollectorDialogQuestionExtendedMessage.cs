using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record TaxCollectorDialogQuestionExtendedMessage : TaxCollectorDialogQuestionBasicMessage
	{
        public short pods;
        public short prospecting;
        public short wisdom;
        public sbyte taxCollectorsCount;

        public new const int Id = 5615;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public TaxCollectorDialogQuestionExtendedMessage()
        {
        }
        public TaxCollectorDialogQuestionExtendedMessage(string guildName, short pods, short prospecting, short wisdom, sbyte taxCollectorsCount) : base(guildName)
        {
            this.pods = pods;
            this.prospecting = prospecting;
            this.wisdom = wisdom;
            this.taxCollectorsCount = taxCollectorsCount;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(pods);
            writer.WriteShort(prospecting);
            writer.WriteShort(wisdom);
            writer.WriteSByte(taxCollectorsCount);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            pods = reader.ReadShort();
            if (pods < 0)
                throw new Exception("Forbidden value on pods = " + pods + ", it doesn't respect the following condition : pods < 0");
            prospecting = reader.ReadShort();
            if (prospecting < 0)
                throw new Exception("Forbidden value on prospecting = " + prospecting + ", it doesn't respect the following condition : prospecting < 0");
            wisdom = reader.ReadShort();
            if (wisdom < 0)
                throw new Exception("Forbidden value on wisdom = " + wisdom + ", it doesn't respect the following condition : wisdom < 0");
            taxCollectorsCount = reader.ReadSByte();
            if (taxCollectorsCount < 0)
                throw new Exception("Forbidden value on taxCollectorsCount = " + taxCollectorsCount + ", it doesn't respect the following condition : taxCollectorsCount < 0");
		}
	}
}
