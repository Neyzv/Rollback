using System.Drawing;
using Rollback.Protocol.Enums;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Spells;
using Rollback.World.Game.World.Maps.CellsZone;

namespace Rollback.World.Game.Fights.Triggers.Types
{
    public sealed class Glyph : SpellTriggerMark
    {
        public override GameActionMarkTypeEnum Type =>
            GameActionMarkTypeEnum.GLYPH;

        public short Duration { get; set; }

        public Glyph(short id, TriggerType? triggerType, Zone zone, Color color, FightActor caster, Spell spell, short duration)
            : base(id, triggerType, zone, color, caster, spell) =>
            Duration = duration;

        protected override SpellCast InternalTrigger(FightActor target) =>
            new(Caster, Spell, target.Cell, FightSpellCastCriticalEnum.NORMAL);
    }
}
