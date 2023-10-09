using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;


namespace Rollback.Protocol.Messages
{
	public record ExchangeStartOkHumanVendorMessage : Message
	{
        public int sellerId;
        public ObjectItemToSell[] objectsInfos;

        public const int Id = 5767;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeStartOkHumanVendorMessage()
        {
        }
        public ExchangeStartOkHumanVendorMessage(int sellerId, ObjectItemToSell[] objectsInfos)
        {
            this.sellerId = sellerId;
            this.objectsInfos = objectsInfos;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(sellerId);
            writer.WriteUShort((ushort)objectsInfos.Length);
            foreach (var entry in objectsInfos)
            {
                 entry.Serialize(writer);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            sellerId = reader.ReadInt();
            if (sellerId < 0)
                throw new Exception("Forbidden value on sellerId = " + sellerId + ", it doesn't respect the following condition : sellerId < 0");
            var limit = reader.ReadUShort();
            objectsInfos = new ObjectItemToSell[limit];
            for (int i = 0; i < limit; i++)
            {
                 objectsInfos[i] = new ObjectItemToSell();
                 objectsInfos[i].Deserialize(reader);
            }
		}
	}
}
