using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Types
{
    public record FightTemporaryBoostStateEffect : FightTemporaryBoostEffect
	{ 
		public short stateId;
		public new const short Id = 214;
		public override short TypeId
		{
			get { return Id; }
		}
		public FightTemporaryBoostStateEffect()
		{
		}
		public FightTemporaryBoostStateEffect(int uid, int targetId, short turnDuration, sbyte dispelable, short spellId, short delta, short stateId) : base(uid, targetId, turnDuration, dispelable, spellId, delta)
		{
			this.stateId = stateId;
		}
		public override void Serialize(BigEndianWriter writer)
		{
			base.Serialize(writer);
			writer.WriteShort(stateId);
		}
		public override void Deserialize(BigEndianReader reader)
		{
			base.Deserialize(reader);
			stateId = reader.ReadShort();
		}
	}
}