using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Fights;
using Rollback.World.Game.Fights.Buffs.Types;
using Rollback.World.Game.World.Maps.CellsZone;

namespace Rollback.World.Game.Effects.Handlers.Spells.Damages
{
    [Identifier(EffectId.EffectDamageEarth), Identifier(EffectId.EffectDamageAir), Identifier(EffectId.EffectDamageFire),
        Identifier(EffectId.EffectDamageNeutral), Identifier(EffectId.EffectDamageWater), Identifier(EffectId.EffectDamageCaster)]
    public sealed class DamageEffectHandler : SpellEffectHandler
    {
        public DamageEffectHandler(EffectBase effect, List<FightActor> target, SpellCast cast, Zone zone) : base(effect, target, cast, zone) { }

        protected override void InternalApply(FightActor fighter)
        {
            if (Effect.Duration is not 0)
                fighter.AddTriggerBuff(BuffTriggerType.OnTurnBegin, this, OnTurnBegin);
            else
            {
                var effectInteger = GenerateEffect();

                if (effectInteger is not null)
                    (Effect.Id is EffectId.EffectDamageCaster ? Cast.Caster : fighter).InflictDamage(Cast.Caster, new Damage(Cast.Spell, effectInteger, Zone));
            }
        }

        private static void OnTurnBegin(TriggerBuff buff, FightActor trigger, BuffTriggerType type, object? token)
        {
            var effectInteger = buff.Handler.GenerateEffect();
            if (effectInteger is not null)
                (buff.Handler.Effect.Id is EffectId.EffectDamageCaster ? buff.Handler.Cast.Caster : buff.Target).InflictDamage(buff.Handler.Cast.Caster, new Damage(buff.Handler.Cast.Spell, effectInteger));
        }
    }
}
