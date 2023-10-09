using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record ExchangeObjectPutInBagMessage : ExchangeObjectMessage
	{
        public ObjectItem @object;

        public new const int Id = 6009;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeObjectPutInBagMessage()
        {
        }
        public ExchangeObjectPutInBagMessage(bool remote, ObjectItem @object) : base(remote)
        {
            this.@object = @object;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            @object.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            @object = new ObjectItem();
            @object.Deserialize(reader);
		}
	}
}
