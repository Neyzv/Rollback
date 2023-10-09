using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record ExchangeBidHouseInListRemovedMessage : Message
	{
        public int itemUID;

        public const int Id = 5950;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeBidHouseInListRemovedMessage()
        {
        }
        public ExchangeBidHouseInListRemovedMessage(int itemUID)
        {
            this.itemUID = itemUID;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(itemUID);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            itemUID = reader.ReadInt();
		}
	}
}
