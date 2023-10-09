using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record ExchangeObjectAddedMessage : ExchangeObjectMessage
	{
        public ObjectItem @object;

        public new const int Id = 5516;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeObjectAddedMessage()
        {
        }
        public ExchangeObjectAddedMessage(bool remote, ObjectItem @object) : base(remote)
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
