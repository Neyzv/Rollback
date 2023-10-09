using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record TaxCollectorAttackedResultMessage : Message
	{
        public bool deadOrAlive;
        public TaxCollectorBasicInformations basicInfos;

        public const int Id = 5635;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public TaxCollectorAttackedResultMessage()
        {
        }
        public TaxCollectorAttackedResultMessage(bool deadOrAlive, TaxCollectorBasicInformations basicInfos)
        {
            this.deadOrAlive = deadOrAlive;
            this.basicInfos = basicInfos;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteBoolean(deadOrAlive);
            basicInfos.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            deadOrAlive = reader.ReadBoolean();
            basicInfos = new TaxCollectorBasicInformations();
            basicInfos.Deserialize(reader);
		}
	}
}
