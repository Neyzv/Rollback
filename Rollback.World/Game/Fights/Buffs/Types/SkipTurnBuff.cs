using Rollback.Protocol.Types;
using Rollback.World.Game.Effects.Handlers.Spells;

namespace Rollback.World.Game.Fights.Buffs.Types
{
    public sealed class SkipTurnBuff : Buff
    {
        public override AbstractFightDispellableEffect AbstractFightDispellableEffect =>
            new FightTemporaryBoostEffect(Id, Target.Id, Duration, (sbyte)Handler.Dispellable, Handler.Cast.Spell.Id, 0);

        public SkipTurnBuff(int id, SpellEffectHandler handler, FightActor target, short? customActionId = null) : base(id, handler, target, customActionId) { }
    }
}
