using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record GameRolePlayMerchantInformations : GameRolePlayNamedActorInformations
	{ 
		public int sellType;
		public new const short Id = 129;
		public override short TypeId
		{
			get { return Id; }
		}
		public GameRolePlayMerchantInformations()
		{
		}
		public GameRolePlayMerchantInformations(int contextualId, EntityLook look, EntityDispositionInformations disposition, string name, int sellType) : base(contextualId, look, disposition, name)
		{
			this.sellType = sellType;
		}
		public override void Serialize(BigEndianWriter writer)
		{
			base.Serialize(writer);
			writer.WriteInt(sellType);
		}
		public override void Deserialize(BigEndianReader reader)
		{
			base.Deserialize(reader);
			sellType = reader.ReadInt();
			if (sellType < 0)
				throw new Exception("Forbidden value on sellType = " + sellType + ", it doesn't respect the following condition : sellType < 0");
		}
	}
}