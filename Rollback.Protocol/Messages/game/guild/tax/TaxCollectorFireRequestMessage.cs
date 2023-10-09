using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record TaxCollectorFireRequestMessage : Message
	{
        public int collectorId;

        public const int Id = 5682;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public TaxCollectorFireRequestMessage()
        {
        }
        public TaxCollectorFireRequestMessage(int collectorId)
        {
            this.collectorId = collectorId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(collectorId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            collectorId = reader.ReadInt();
		}
	}
}
