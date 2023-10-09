using Rollback.Protocol.Types;
using Rollback.World.Game.Effects.Handlers.Spells;
using Rollback.World.Game.Looks;

namespace Rollback.World.Game.Fights.Buffs.Types
{
    public sealed class SkinBuff : Buff
    {
        public ActorLook Look { get; }

        public override AbstractFightDispellableEffect AbstractFightDispellableEffect =>
            new FightTemporaryBoostEffect(Id, Target.Id, Duration, (sbyte)Handler.Dispellable, Handler.Cast.Spell.Id, 0);

        public SkinBuff(int id, SpellEffectHandler handler, FightActor target, ActorLook look, short? customActionId = null)
            : base(id, handler, target, customActionId) =>
            Look = look;

        public override void Apply() =>
            Target.UpdateLook(Handler.Cast.Caster);

        public override void Dispell() =>
            Target.UpdateLook(Handler.Cast.Caster);
    }
}
