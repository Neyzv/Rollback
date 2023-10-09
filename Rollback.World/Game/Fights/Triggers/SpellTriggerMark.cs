using System.Drawing;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Spells;
using Rollback.World.Game.World.Maps.CellsZone;
using Rollback.World.Handlers.Fights;

namespace Rollback.World.Game.Fights.Triggers
{
    public abstract class SpellTriggerMark : TriggerMark
    {
        public Spell Spell { get; }

        protected SpellTriggerMark(short id, TriggerType? triggerType, Zone zone, Color color, FightActor caster, Spell spell)
            : base(id, triggerType, zone, color, caster) =>
            Spell = spell;

        protected abstract SpellCast InternalTrigger(FightActor target);

        public override SpellCast Trigger(FightActor target)
        {
            target.Team!.Fight.Send(FightHandler.SendGameActionFightTriggerGlyphTrapMessage, new object[] { this, target });

            return InternalTrigger(target);
        }
    }
}
