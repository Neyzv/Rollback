using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record ExchangeMultiCraftSetCrafterCanUseHisRessourcesMessage : Message
	{
        public bool allow;

        public const int Id = 6021;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeMultiCraftSetCrafterCanUseHisRessourcesMessage()
        {
        }
        public ExchangeMultiCraftSetCrafterCanUseHisRessourcesMessage(bool allow)
        {
            this.allow = allow;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteBoolean(allow);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            allow = reader.ReadBoolean();
		}
	}
}
