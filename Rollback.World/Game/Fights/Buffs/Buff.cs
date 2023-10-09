using Rollback.Protocol.Types;
using Rollback.World.Game.Effects.Handlers.Spells;

namespace Rollback.World.Game.Fights.Buffs
{
    public abstract class Buff
    {
        public int Id { get; }

        private readonly short? _customActionId;
        public short ActionId =>
            _customActionId ?? (short)Handler.Effect.Id;

        public SpellEffectHandler Handler { get; }

        public short Duration { get; set; }

        public FightActor Target { get; }

        public abstract AbstractFightDispellableEffect AbstractFightDispellableEffect { get; }

        public Buff(int id, SpellEffectHandler handler, FightActor target, short? customActionId = default)
        {
            _customActionId = customActionId;

            Id = id;
            Handler = handler;

            Duration = (short)(handler.Effect.Duration + (handler.Cast.Caster.Id == target.Id && handler.Target.FirstOrDefault(x => x.Id == target.Id) is not null ? 1 : 0));
            if (Duration is 0)
                Duration = 1;

            Target = target;
        }

        public virtual void Apply() { }

        public virtual void Dispell() { }
    }
}
