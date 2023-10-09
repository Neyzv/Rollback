using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record ExchangeBidHouseGenericItemAddedMessage : Message
	{
        public int objGenericId;

        public const int Id = 5947;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeBidHouseGenericItemAddedMessage()
        {
        }
        public ExchangeBidHouseGenericItemAddedMessage(int objGenericId)
        {
            this.objGenericId = objGenericId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(objGenericId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            objGenericId = reader.ReadInt();
		}
	}
}
