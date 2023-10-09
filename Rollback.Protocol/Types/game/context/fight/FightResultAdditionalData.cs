using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Types
{
    public record FightResultAdditionalData
	{ 
		public const short Id = 191;
		public virtual short TypeId
		{
			get { return Id; }
		}
		public FightResultAdditionalData()
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