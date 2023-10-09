using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Fights;
using Rollback.World.Game.Fights.Buffs.Types;
using Rollback.World.Game.World.Maps.CellsZone;

namespace Rollback.World.Game.Effects.Handlers.Spells.Damages
{
    [Identifier(EffectId.EffectStealHPAir), Identifier(EffectId.EffectStealHPEarth), Identifier(EffectId.EffectStealHPFire),
        Identifier(EffectId.EffectStealHPFix), Identifier(EffectId.EffectStealHPNeutral), Identifier(EffectId.EffectStealHPWater)]
    public sealed class StealHpEffectHandler : SpellEffectHandler
    {
        public StealHpEffectHandler(EffectBase effect, List<FightActor> target, SpellCast cast, Zone zone) : base(effect, target, cast, zone) { }

        protected override void InternalApply(FightActor fighter)
        {
            if (Effect.Duration is not 0)
                fighter.AddTriggerBuff(BuffTriggerType.OnTurnBegin, this, OnTurnBegin);
            else
            {
                var effectInteger = GenerateEffect();
                if (effectInteger is not null)
                {
                    var ignoreBoost = effectInteger.Id is EffectId.EffectStealHPFix;
                    var damage = new Damage(Cast.Spell, effectInteger, ignoreBoost ? default : Zone, !ignoreBoost, !ignoreBoost);

                    fighter.InflictDamage(Cast.Caster, damage);
                    Cast.Caster.Heal(Cast.Caster, (short)Math.Floor(damage.Amount / 2d), applyBoost: false);
                }
            }
        }

        private static void OnTurnBegin(TriggerBuff buff, FightActor trigger, BuffTriggerType type, object? token)
        {
            var effectInteger = buff.Handler.GenerateEffect();
            if (effectInteger is not null)
            {
                var damage = new Damage(buff.Handler.Cast.Spell, effectInteger);
                buff.Target.InflictDamage(buff.Handler.Cast.Caster, damage);
                buff.Handler.Cast.Caster.Heal(buff.Handler.Cast.Caster, (short)Math.Floor(damage.Amount / 2d), applyBoost: false);
            }
        }
    }
}
