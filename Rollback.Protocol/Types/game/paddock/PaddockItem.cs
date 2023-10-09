using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Types
{
    public record PaddockItem : ObjectItemInRolePlay
    {
        public ItemDurability durability;
        public new const short Id = 185;
        public override short TypeId
        {
            get { return Id; }
        }
        public PaddockItem()
        {
        }
        public PaddockItem(short cellId, short objectGID, ItemDurability durability) : base(cellId, objectGID)
        {
            this.durability = durability;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            durability.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            durability = new ItemDurability();
            durability.Deserialize(reader);
        }
    }
}