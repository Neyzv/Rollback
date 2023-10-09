using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record StorageObjectUpdateMessage : Message
	{
        public ObjectItem @object;

        public const int Id = 5647;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public StorageObjectUpdateMessage()
        {
        }
        public StorageObjectUpdateMessage(ObjectItem @object)
        {
            this.@object = @object;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            @object.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            @object = new ObjectItem();
            @object.Deserialize(reader);
		}
	}
}
