using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record FightTriggeredEffect : AbstractFightDispellableEffect
	{ 
		public sbyte trigger;
        public int arg1;
        public int arg2;
        public int arg3;
		public new const short Id = 210;
		public override short TypeId
		{
			get { return Id; }
		}
		public FightTriggeredEffect()
		{
		}
		public FightTriggeredEffect(int uid, int targetId, short turnDuration, sbyte dispelable, short spellId, sbyte trigger, int arg1, int arg2, int arg3) : base(uid, targetId, turnDuration, dispelable, spellId)
		{
			this.trigger = trigger;
			this.arg1 = arg1;
			this.arg2 = arg2;
			this.arg3 = arg3;
		}
		public override void Serialize(BigEndianWriter writer)
		{
			base.Serialize(writer);
			writer.WriteSByte(trigger);
			writer.WriteInt(arg1);
			writer.WriteInt(arg2);
			writer.WriteInt(arg3);
		}
		public override void Deserialize(BigEndianReader reader)
		{
			base.Deserialize(reader);
			trigger = reader.ReadSByte();
			if (trigger < 0)
				throw new Exception("Forbidden value on trigger = " + trigger + ", it doesn't respect the following condition : trigger < 0");
			arg1 = reader.ReadInt();
			arg2 = reader.ReadInt();
			arg3 = reader.ReadInt();

		}
	}
}