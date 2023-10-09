using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Fights;
using Rollback.World.Game.World.Maps.CellsZone;

namespace Rollback.World.Game.Effects.Handlers.Spells.Buffs
{
    [Identifier(EffectId.EffectRemoveSpellEffects)]
    public sealed class RemoveSpellEffectsEffectHandler : SpellEffectHandler
    {
        public RemoveSpellEffectsEffectHandler(EffectBase effect, List<FightActor> target, SpellCast cast, Zone zone) : base(effect, target, cast, zone) { }

        protected override void InternalApply(FightActor fighter)
        {
            var integerEffect = GenerateEffect();
            if (integerEffect is not null)
                fighter.DispellBuffs(Cast.Caster, x => x.Handler.Cast.Spell.Id == integerEffect.Value);
        }
    }
}
