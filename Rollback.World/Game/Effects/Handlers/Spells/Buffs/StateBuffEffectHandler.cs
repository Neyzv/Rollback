using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Effects.Types;
using Rollback.World.Game.Fights;
using Rollback.World.Game.World.Maps.CellsZone;

namespace Rollback.World.Game.Effects.Handlers.Spells.Buffs
{
    [Identifier(EffectId.EffectAddState)]
    public sealed class StateBuffEffectHandler : SpellEffectHandler
    {
        private static readonly HashSet<SpellState> _dispellableStates = new()
        {
            SpellState.Invulnerable,
            SpellState.Saoul,
        };

        public StateBuffEffectHandler(EffectBase effect, List<FightActor> target, SpellCast cast, Zone zone) : base(effect, target, cast, zone)
        {
            if (Effect is EffectDice effectDice)
                Dispellable = _dispellableStates.Contains((SpellState)effectDice.Value) ? FightDispellable.Dispellable : FightDispellable.DispellableByDeath;
        }

        protected override void InternalApply(FightActor fighter)
        {
            if (Effect is EffectDice effectDice)
            {
                var state = (SpellState)effectDice.Value;

                if (Enum.IsDefined(state))
                    fighter.AddStateBuff(this, state);
            }
        }
    }
}
