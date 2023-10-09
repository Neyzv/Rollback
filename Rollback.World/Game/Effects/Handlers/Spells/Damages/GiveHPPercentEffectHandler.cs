using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Fights;
using Rollback.World.Game.World.Maps.CellsZone;

namespace Rollback.World.Game.Effects.Handlers.Spells.Damages
{
    [Identifier(EffectId.EffectGiveHPPercent)]
    public sealed class GiveHPPercentEffectHandler : SpellEffectHandler
    {
        private readonly short _healAmount;

        public GiveHPPercentEffectHandler(EffectBase effect, List<FightActor> target, SpellCast cast, Zone zone) : base(effect, target, cast, zone)
        {
            var effectInteger = GenerateEffect();
            if (effectInteger is not null && Target.Count is not 0)
            {
                _healAmount = (short)Math.Floor(Cast.Caster.Stats.Health.Actual * effectInteger.Value / 100d);
                Cast.Caster.InflictDamage(Cast.Caster, new(_healAmount, applyBoost: false, applyResistance: false));
            }
        }

        protected override void InternalApply(FightActor fighter)
        {
            if (_healAmount > 0)
                fighter.Heal(Cast.Caster, _healAmount, Zone, false);
        }
    }
}
