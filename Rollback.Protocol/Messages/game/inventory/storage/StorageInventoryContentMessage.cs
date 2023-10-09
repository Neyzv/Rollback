using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record StorageInventoryContentMessage : InventoryContentMessage
	{
        public new const int Id = 5646;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public StorageInventoryContentMessage()
        {
        }
        public StorageInventoryContentMessage(ObjectItem[] objects, int kamas) : base(objects, kamas)
        {
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
		}
	}
}
