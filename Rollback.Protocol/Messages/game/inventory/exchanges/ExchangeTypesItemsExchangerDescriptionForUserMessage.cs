using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record ExchangeTypesItemsExchangerDescriptionForUserMessage : Message
	{
        public BidExchangerObjectInfo[] itemTypeDescriptions;

        public const int Id = 5752;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeTypesItemsExchangerDescriptionForUserMessage()
        {
        }
        public ExchangeTypesItemsExchangerDescriptionForUserMessage(BidExchangerObjectInfo[] itemTypeDescriptions)
        {
            this.itemTypeDescriptions = itemTypeDescriptions;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)itemTypeDescriptions.Length);
            foreach (var entry in itemTypeDescriptions)
            {
                 entry.Serialize(writer);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            itemTypeDescriptions = new BidExchangerObjectInfo[limit];
            for (int i = 0; i < limit; i++)
            {
                 itemTypeDescriptions[i] = new BidExchangerObjectInfo();
                 itemTypeDescriptions[i].Deserialize(reader);
            }
		}
	}
}
