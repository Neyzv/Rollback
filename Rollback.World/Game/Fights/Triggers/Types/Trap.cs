using System.Drawing;
using Rollback.Protocol.Enums;
using Rollback.World.Game.Spells;
using Rollback.World.Game.World.Maps.CellsZone;

namespace Rollback.World.Game.Fights.Triggers.Types
{
    public sealed class Trap : SpellTriggerMark
    {
        public override GameActionMarkTypeEnum Type =>
            GameActionMarkTypeEnum.TRAP;

        public Trap(short id, Zone zone, Color color, FightActor caster, Spell spell) : base(id, CustomEnums.TriggerType.Move, zone, color, caster, spell) =>
            Visibility = GameActionFightInvisibilityStateEnum.INVISIBLE;

        protected override SpellCast InternalTrigger(FightActor target)
        {
            target.Team!.Fight.RemoveTrigger(this);

            return new SpellCast(Caster, Spell, Zone.CenterCell, FightSpellCastCriticalEnum.NORMAL);
        }
    }
}
