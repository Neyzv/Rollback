using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record ExchangeTypesExchangerDescriptionForUserMessage : Message
	{
        public int[] typeDescription;

        public const int Id = 5765;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeTypesExchangerDescriptionForUserMessage()
        {
        }
        public ExchangeTypesExchangerDescriptionForUserMessage(int[] typeDescription)
        {
            this.typeDescription = typeDescription;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)typeDescription.Length);
            foreach (var entry in typeDescription)
            {
                 writer.WriteInt(entry);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            typeDescription = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 typeDescription[i] = reader.ReadInt();
            }
		}
	}
}
