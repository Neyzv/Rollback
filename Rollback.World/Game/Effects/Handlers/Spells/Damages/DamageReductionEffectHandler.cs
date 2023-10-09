using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Fights;
using Rollback.World.Game.Fights.Buffs.Types;
using Rollback.World.Game.World.Maps.CellsZone;
using Rollback.World.Handlers.Fights;

namespace Rollback.World.Game.Effects.Handlers.Spells.Damages
{
    [Identifier(EffectId.EffectAddArmorDamageReduction)]
    [Identifier(EffectId.EffectAddGlobalDamageReduction105)]
    public sealed class DamageReductionEffectHandler : SpellEffectHandler
    {
        public DamageReductionEffectHandler(EffectBase effect, List<FightActor> target, SpellCast cast, Zone zone) : base(effect, target, cast, zone) { }

        protected override void InternalApply(FightActor fighter)
        {
            if (Effect.Duration is not 0)
            {
                fighter.DispellBuffs(Cast.Caster, x => x.Handler.Cast.Spell.Id == Cast.Spell.Id && x.Duration < Effect.Duration);
                fighter.AddTriggerBuff(BuffTriggerType.OnDamaged, this, OnBeforeDamaged);
            }
        }

        private static void OnBeforeDamaged(TriggerBuff buff, FightActor trigger, BuffTriggerType type, object? token)
        {
            var effectInteger = buff.Handler.GenerateEffect();
            if (effectInteger is not null && token is Damage damage && damage.Amount is not 0 && damage.ApplyResistance && damage.Effect?.Duration is 0)
            {
                var reduction = Math.Min(damage.Amount, FightFormulas.CalculateArmorReduction(buff.Target, effectInteger.Value));
                damage.Amount -= reduction;

                buff.Target.Team!.Fight.Send(FightHandler.SendGameActionFightReduceDamagesMessage, new object[] { buff.Handler.Cast.Caster, buff.Target, reduction });
            }
        }
    }
}
