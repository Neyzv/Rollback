using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Effects.Types;
using Rollback.World.Game.Fights;
using Rollback.World.Game.Fights.Buffs.Types;
using Rollback.World.Game.World.Maps.CellsZone;

namespace Rollback.World.Game.Effects.Handlers.Spells.Buffs
{
    [Identifier(EffectId.EffectDispelState)]
    public sealed class DispelStateEffectHandler : SpellEffectHandler
    {
        private static readonly HashSet<SpellState> _cantDispellState = new()
        {
            SpellState.Invulnerable
        };

        public DispelStateEffectHandler(EffectBase effect, List<FightActor> target, SpellCast cast, Zone zone)
            : base(effect, target, cast, zone) =>
            Dispellable = Effect is EffectDice effectDice && _cantDispellState.Contains((SpellState)effectDice.Value) ?
            FightDispellable.NotDispellable : FightDispellable.DispellableByDeath;

        protected override void InternalApply(FightActor fighter)
        {
            if (Effect.Duration is 0)
            {
                if (GenerateEffect() is { } effectInteger)
                    fighter.DispellBuffs(Cast.Caster, x => x is StateBuff stateBuff && stateBuff.State == (SpellState)effectInteger.Value);
            }
            else
                fighter.AddTriggerBuff(BuffTriggerType.OnTurnBegin, this, OnTurnBegined);
        }

        private static void OnTurnBegined(TriggerBuff buff, FightActor trigger, BuffTriggerType type, object? token)
        {
            if (buff.Handler.GenerateEffect() is { } effectInteger)
                buff.Target.DispellBuffs(buff.Handler.Cast.Caster, x => x is StateBuff stateBuff && stateBuff.State == (SpellState)effectInteger.Value);
        }
    }
}
