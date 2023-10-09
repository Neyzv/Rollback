using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Types
{
    public record FightTemporaryBoostEffect : AbstractFightDispellableEffect
	{ 
		public short delta;
		public new const short Id = 209;
		public override short TypeId
		{
			get { return Id; }
		}
		public FightTemporaryBoostEffect()
		{
		}
		public FightTemporaryBoostEffect(int uid, int targetId, short turnDuration, sbyte dispelable, short spellId, short delta) : base(uid, targetId, turnDuration, dispelable, spellId)
		{
			this.delta = delta;
		}
		public override void Serialize(BigEndianWriter writer)
		{
			base.Serialize(writer);
			writer.WriteShort(delta);
		}
		public override void Deserialize(BigEndianReader reader)
		{
			base.Deserialize(reader);
			delta = reader.ReadShort();
		}
	}
}