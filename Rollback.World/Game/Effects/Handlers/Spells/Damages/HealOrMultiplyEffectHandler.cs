using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Effects.Types;
using Rollback.World.Game.Fights;
using Rollback.World.Game.Fights.Buffs.Types;
using Rollback.World.Game.World.Maps.CellsZone;

namespace Rollback.World.Game.Effects.Handlers.Spells.Damages
{
    [Identifier(EffectId.EffectHealOrMultiply)]
    public sealed class HealOrMultiplyEffectHandler : SpellEffectHandler
    {
        public HealOrMultiplyEffectHandler(EffectBase effect, List<FightActor> target, SpellCast cast, Zone zone) : base(effect, target, cast, zone) { }

        protected override void InternalApply(FightActor fighter) =>
            fighter.AddTriggerBuff(BuffTriggerType.BeforeDamaged, this, OnBeforeDamaged);

        private static void OnBeforeDamaged(TriggerBuff buff, FightActor trigger, BuffTriggerType type, object? token)
        {
            if (token is Damage damage && buff.Handler.Effect is EffectDice effectDice)
            {
                if (Random.Shared.Next(101) <= effectDice.Value)
                    damage.Amount *= effectDice.DiceNum;
                else
                {
                    buff.Target.Heal(buff.Target, (short)Math.Floor(damage.Amount * (effectDice.DiceFace / 100d)));
                    damage.Amount = 0;
                }
            }
        }
    }
}
