using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Effects.Types;
using Rollback.World.Game.Fights;
using Rollback.World.Game.Fights.Buffs.Types;
using Rollback.World.Game.World.Maps.CellsZone;

namespace Rollback.World.Game.Effects.Handlers.Spells.Buffs
{
    [Identifier(EffectId.EffectDodge)]
    public sealed class DodgeEffectHandler : SpellEffectHandler
    {
        public DodgeEffectHandler(EffectBase effect, List<FightActor> target, SpellCast cast, Zone zone) : base(effect, target, cast, zone) { }

        protected override void InternalApply(FightActor fighter) =>
            fighter.AddTriggerBuff(BuffTriggerType.BeforeDamaged, this, OnBeforeDamaged);

        private static void OnBeforeDamaged(TriggerBuff buff, FightActor trigger, BuffTriggerType type, object? token)
        {
            if (token is Damage damage && buff.Handler.Effect is EffectDice effectDice && buff.Target.Cell.Point.GetAdjacentCells().Any(x => x.CellId == trigger.Cell.Id))
            {
                if (Random.Shared.Next(effectDice.DiceNum + 1) <= effectDice.DiceNum)
                {
                    damage.Amount = 0;
                    buff.Target.Push(trigger, trigger.Cell.Point.OrientationTo(buff.Target.Cell.Point), effectDice.DiceFace, false);
                }
            }
        }
    }
}
