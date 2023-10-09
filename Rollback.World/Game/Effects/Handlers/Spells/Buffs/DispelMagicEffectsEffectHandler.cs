using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Fights;
using Rollback.World.Game.World.Maps.CellsZone;

namespace Rollback.World.Game.Effects.Handlers.Spells.Buffs
{
    [Identifier(EffectId.EffectDispelMagicEffects)]
    public sealed class DispelMagicEffectsEffectHandler : SpellEffectHandler
    {
        public DispelMagicEffectsEffectHandler(EffectBase effect, List<FightActor> target, SpellCast cast, Zone zone) : base(effect, target, cast, zone) { }

        protected override void InternalApply(FightActor fighter)
        {
            fighter.DispellBuffs(Cast.Caster, x => x.Handler.Dispellable <= FightDispellable.ReallyNotDispellable);
            fighter.Trigger(BuffTriggerType.OnDispelled, Cast.Caster);
        }
    }
}
