using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record ExchangeItemPaymentForCraftMessage : Message
	{
        public bool onlySuccess;
        public ObjectItemNotInContainer @object;

        public const int Id = 5831;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeItemPaymentForCraftMessage()
        {
        }
        public ExchangeItemPaymentForCraftMessage(bool onlySuccess, ObjectItemNotInContainer @object)
        {
            this.onlySuccess = onlySuccess;
            this.@object = @object;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteBoolean(onlySuccess);
            @object.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            onlySuccess = reader.ReadBoolean();
            @object = new ObjectItemNotInContainer();
            @object.Deserialize(reader);
		}
	}
}
