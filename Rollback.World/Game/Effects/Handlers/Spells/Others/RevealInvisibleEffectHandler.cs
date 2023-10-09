using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Fights;
using Rollback.World.Game.Fights.Buffs.Types;
using Rollback.World.Game.Fights.Triggers;
using Rollback.World.Game.World.Maps.CellsZone;

namespace Rollback.World.Game.Effects.Handlers.Spells.Others
{
    [Identifier(EffectId.EffectRevealsInvisible)]
    public sealed class RevealInvisibleEffectHandler : SpellEffectHandler
    {
        public RevealInvisibleEffectHandler(EffectBase effect, List<FightActor> target, SpellCast cast, Zone zone) : base(effect, target, cast, zone) =>
            Target = new() { Cast.Caster };

        protected override void InternalApply(FightActor fighter)
        {
            foreach (var trap in fighter.Team!.Fight.GetTriggers<TriggerMark>(x => !x.Caster.IsFriendlyWith(fighter) && x.Zone.AffectedCells.Any(y => Zone.AffectedCells.ContainsKey(y.Key))))
                trap.ChangeVisibility(true);

            foreach (var actor in fighter.Team.OpposedTeam.GetFighters<FightActor>(x => Zone.AffectedCells.ContainsKey(x.Cell.Id)))
                actor.DispellBuffs(fighter, x => x is InvisibilityBuff);
        }
    }
}
