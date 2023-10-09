using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Fights;
using Rollback.World.Game.Fights.Buffs.Types;
using Rollback.World.Game.World.Maps.CellsZone;

namespace Rollback.World.Game.Effects.Handlers.Spells.Damages
{
    [Identifier(EffectId.EffectHealHP81), Identifier(EffectId.EffectHealHP108), Identifier(EffectId.EffectHealHP143)]
    public sealed class HealEffectHandler : SpellEffectHandler
    {
        public HealEffectHandler(EffectBase effect, List<FightActor> target, SpellCast cast, Zone zone) : base(effect, target, cast, zone) { }

        protected override void InternalApply(FightActor fighter)
        {
            if (Effect.Duration is not 0)
                fighter.AddTriggerBuff(BuffTriggerType.OnTurnBegin, this, OnTurnBegin);
            else
            {
                var effectInteger = GenerateEffect();

                if (effectInteger is not null)
                    fighter.Heal(Cast.Caster, effectInteger.Value, Zone);
            }
        }

        private static void OnTurnBegin(TriggerBuff buff, FightActor trigger, BuffTriggerType type, object? token)
        {
            var effectInteger = buff.Handler.GenerateEffect();
            if (effectInteger is not null)
                buff.Target.Heal(buff.Handler.Cast.Caster, effectInteger.Value);
        }
    }
}
