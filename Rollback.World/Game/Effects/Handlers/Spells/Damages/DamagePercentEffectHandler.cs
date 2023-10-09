using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Fights;
using Rollback.World.Game.Fights.Buffs.Types;
using Rollback.World.Game.World.Maps.CellsZone;

namespace Rollback.World.Game.Effects.Handlers.Spells.Damages
{
    [Identifier(EffectId.EffectDamagePercentAir), Identifier(EffectId.EffectDamagePercentEarth), Identifier(EffectId.EffectDamagePercentFire),
        Identifier(EffectId.EffectDamagePercentWater), Identifier(EffectId.EffectDamagePercentNeutral), Identifier(EffectId.EffectDamagePercentNeutral671)]
    public sealed class DamagePercentEffectHandler : SpellEffectHandler
    {
        public DamagePercentEffectHandler(EffectBase effect, List<FightActor> target, SpellCast cast, Zone zone) : base(effect, target, cast, zone) { }

        protected override void InternalApply(FightActor fighter)
        {
            if (Effect.Duration is not 0)
                fighter.AddTriggerBuff(BuffTriggerType.OnTurnBegin, this, OnTurnBegin);
            else
            {
                var effectInteger = GenerateEffect();

                if (effectInteger is not null)
                {
                    effectInteger.Value = (short)Math.Floor(Cast.Caster.Stats.Health.Actual * effectInteger.Value / 100d);

                    fighter.InflictDamage(Cast.Caster, new Damage(Cast.Spell, effectInteger, Zone, false, Effect.Id is not EffectId.EffectDamagePercentNeutral671));
                }
            }
        }

        private static void OnTurnBegin(TriggerBuff buff, FightActor trigger, BuffTriggerType type, object? token)
        {
            var effectInteger = buff.Handler.GenerateEffect();

            if (effectInteger is not null)
            {
                effectInteger.Value = (short)Math.Floor(buff.Handler.Cast.Caster.Stats.Health.Actual * effectInteger.Value / 100d);

                buff.Target.InflictDamage(buff.Handler.Cast.Caster, new Damage(buff.Handler.Cast.Spell, effectInteger, applyBoost: false, applyResistance: buff.Handler.Effect.Id is not EffectId.EffectDamagePercentNeutral671));
            }
        }
    }
}
