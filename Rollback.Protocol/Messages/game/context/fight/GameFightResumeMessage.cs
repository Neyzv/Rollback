using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;


namespace Rollback.Protocol.Messages
{
	public record GameFightResumeMessage : GameFightSpectateMessage
	{
        public GameFightSpellCooldown[] spellCooldowns;
        public sbyte summonCount;

        public new const int Id = 6067;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameFightResumeMessage()
        {
        }
        public GameFightResumeMessage(FightDispellableEffectExtendedInformations[] effects, GameFightSpellCooldown[] spellCooldowns, sbyte summonCount) : base(effects)
        {
            this.spellCooldowns = spellCooldowns;
            this.summonCount = summonCount;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUShort((ushort)spellCooldowns.Length);
            foreach (var entry in spellCooldowns)
            {
                 entry.Serialize(writer);
            }
            writer.WriteSByte(summonCount);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            spellCooldowns = new GameFightSpellCooldown[limit];
            for (int i = 0; i < limit; i++)
            {
                 spellCooldowns[i] = new GameFightSpellCooldown();
                 spellCooldowns[i].Deserialize(reader);
            }
            summonCount = reader.ReadSByte();
            if (summonCount < 0)
                throw new Exception("Forbidden value on summonCount = " + summonCount + ", it doesn't respect the following condition : summonCount < 0");
		}
	}
}
