using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Fights;
using Rollback.World.Game.Fights.Buffs.Types;
using Rollback.World.Game.World.Maps.CellsZone;

namespace Rollback.World.Game.Effects.Handlers.Spells.Buffs
{
    [Identifier(EffectId.EffectDamageIntercept)]
    public sealed class DamageInterceptEffectHandler : SpellEffectHandler
    {
        public DamageInterceptEffectHandler(EffectBase effect, List<FightActor> target, SpellCast cast, Zone zone)
            : base(effect, target, cast, zone) =>
            Dispellable = FightDispellable.DispellableByDeath;

        protected override void InternalApply(FightActor fighter)
        {
            if (fighter.GetBuff<TriggerBuff>(x => x.Handler.Effect.Id == EffectId.EffectDamageIntercept) is null)
                fighter.AddTriggerBuff(BuffTriggerType.BeforeDamaged, this, OnBeforeDamaged);
        }

        private static void OnBeforeDamaged(TriggerBuff buff, FightActor trigger, BuffTriggerType triggerType, object? token)
        {
            if (token is Damage damage)
            {
                buff.Handler.Cast.Caster.ExchangePositions(buff.Target);

                damage.ApplyBoost = false;
                damage.ApplyResistance = false;

                buff.Handler.Cast.Caster.InflictDamage(buff.Target, damage);
                damage.Amount = 0;
            }
        }
    }
}
