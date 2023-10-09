using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Effects.Handlers.Spells;

namespace Rollback.World.Game.Fights.Buffs.Types
{
    public sealed class StateBuff : Buff
    {
        public SpellState State { get; }

        public override AbstractFightDispellableEffect AbstractFightDispellableEffect =>
            new FightTemporaryBoostStateEffect(Id, Target.Id, Duration, (sbyte)Handler.Dispellable, Handler.Cast.Spell.Id, 1, (short)State);

        public StateBuff(int id, SpellEffectHandler handler, FightActor target, SpellState state, short? customActionId = null) : base(id, handler, target, customActionId) =>
            State = state;
    }
}
