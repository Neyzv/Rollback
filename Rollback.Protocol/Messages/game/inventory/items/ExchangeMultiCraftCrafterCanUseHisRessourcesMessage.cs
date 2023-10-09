using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record ExchangeMultiCraftCrafterCanUseHisRessourcesMessage : Message
	{
        public bool allowed;

        public const int Id = 6020;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeMultiCraftCrafterCanUseHisRessourcesMessage()
        {
        }
        public ExchangeMultiCraftCrafterCanUseHisRessourcesMessage(bool allowed)
        {
            this.allowed = allowed;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteBoolean(allowed);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            allowed = reader.ReadBoolean();
		}
	}
}
