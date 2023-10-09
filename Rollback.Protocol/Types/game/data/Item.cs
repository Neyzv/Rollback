using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Types
{
    public record Item
    {
        public const short Id = 7;
        public virtual short TypeId
        {
            get { return Id; }
        }
        public Item()
        {
        }
        public virtual void Serialize(BigEndianWriter writer)
        {
        }
        public virtual void Deserialize(BigEndianReader reader)
        {
        }
    }
}
			
