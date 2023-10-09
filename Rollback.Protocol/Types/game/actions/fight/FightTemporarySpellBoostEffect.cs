using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record FightTemporarySpellBoostEffect : FightTemporaryBoostEffect
	{ 
		public short boostedSpellId;
		public new const short Id = 207;
		public override short TypeId
		{
			get { return Id; }
		}
		public FightTemporarySpellBoostEffect()
		{
		}
		public FightTemporarySpellBoostEffect(int uid, int targetId, short turnDuration, sbyte dispelable, short spellId, short delta, short boostedSpellId) : base(uid, targetId, turnDuration, dispelable, spellId, delta)
		{
			this.boostedSpellId = boostedSpellId;
		}

		public override void Serialize(BigEndianWriter writer)
		{
			base.Serialize(writer);
			writer.WriteShort(boostedSpellId);
		}
		public override void Deserialize(BigEndianReader reader)
		{
			base.Deserialize(reader);
			boostedSpellId = reader.ReadShort();
			if (boostedSpellId < 0)
				throw new Exception("Forbidden value on boostedSpellId = " + boostedSpellId + ", it doesn't respect the following condition : boostedSpellId < 0");
		}
	}
}