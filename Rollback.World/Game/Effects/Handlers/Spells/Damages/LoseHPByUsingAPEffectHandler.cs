using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Effects.Types;
using Rollback.World.Game.Fights;
using Rollback.World.Game.Fights.Buffs.Types;
using Rollback.World.Game.World.Maps.CellsZone;

namespace Rollback.World.Game.Effects.Handlers.Spells.Damages
{
    [Identifier(EffectId.EffectLoseHPByUsingAP)]
    public sealed class LoseHPByUsingAPEffectHandler : SpellEffectHandler
    {
        public LoseHPByUsingAPEffectHandler(EffectBase effect, List<FightActor> target, SpellCast cast, Zone zone) : base(effect, target, cast, zone) { }

        protected override void InternalApply(FightActor fighter)
        {
            if (Effect.Duration is not 0)
                fighter.AddTriggerBuff(BuffTriggerType.OnTurnEnd, this, OnTurnEnded);
        }

        private static void OnTurnEnded(TriggerBuff buff, FightActor trigger, BuffTriggerType type, object? token)
        {
            if (buff.Handler.Effect is EffectDice effectDice)
            {
                var effectInteger = buff.Handler.GenerateEffect();
                if (effectInteger is not null && buff.Target.Stats.AP.Used > 0)
                {
                    var damage = new Damage(buff.Handler.Cast.Spell, effectInteger)
                    {
                        Amount = (short)(effectDice.DiceFace * Math.Floor(buff.Target.Stats.AP.Used / (double)effectDice.DiceNum))
                    };

                    buff.Target.InflictDamage(buff.Handler.Cast.Caster, damage);
                }
            }
        }
    }
}
