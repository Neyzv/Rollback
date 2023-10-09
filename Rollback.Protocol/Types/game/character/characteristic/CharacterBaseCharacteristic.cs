using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Types
{
    public record CharacterBaseCharacteristic
	{ 
		public short @base;
        public short objectsAndMountBonus;
        public short alignGiftBonus;
        public short contextModif;
		public const short Id = 4;
		public virtual short TypeId
		{
			get { return Id; }
		}
		public CharacterBaseCharacteristic()
		{
		}
		public CharacterBaseCharacteristic(short @base, short objectsAndMountBonus, short alignGiftBonus, short contextModif)
		{
			this.@base = @base;
			this.objectsAndMountBonus = objectsAndMountBonus;
			this.alignGiftBonus = alignGiftBonus;
			this.contextModif = contextModif;
		}
		public virtual void Serialize(BigEndianWriter writer)
		{
            writer.WriteShort(@base);
			writer.WriteShort(objectsAndMountBonus);
			writer.WriteShort(alignGiftBonus);
			writer.WriteShort(contextModif);
		}
		public virtual void Deserialize(BigEndianReader reader)
		{
            @base = reader.ReadShort();
			objectsAndMountBonus = reader.ReadShort();
			alignGiftBonus = reader.ReadShort();
			contextModif = reader.ReadShort();
		}
	}
}