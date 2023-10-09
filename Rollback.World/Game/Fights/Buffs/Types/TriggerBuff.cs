using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Effects.Handlers.Spells;

namespace Rollback.World.Game.Fights.Buffs.Types
{
    public sealed class TriggerBuff : Buff
    {
        private readonly HashSet<BuffTriggerType> _triggersType;
        private readonly Action<TriggerBuff, FightActor, BuffTriggerType, object?> _applyHandler;
        private readonly Action<TriggerBuff>? _removeHandler;

        public override AbstractFightDispellableEffect AbstractFightDispellableEffect
        {
            get
            {
                var values = Handler.Effect.Values;

                return new FightTriggeredEffect(Id,
                    Target.Id,
                    Duration,
                    (sbyte)Handler.Dispellable,
                    Handler.Cast.Spell.Id,
                    (sbyte)_triggersType.First(),
                    values.Length > 0 ? Convert.ToInt32(values[0]) : 0,
                    values.Length > 1 ? Convert.ToInt32(values[1]) : 0,
                    values.Length > 2 ? Convert.ToInt32(values[2]) : 0);
            }
        }

        public TriggerBuff(int id, SpellEffectHandler handler, FightActor target, BuffTriggerType type,
            Action<TriggerBuff, FightActor, BuffTriggerType, object?> applyHandler, Action<TriggerBuff>? removeHandler = default, short? customActionId = null)
            : base(id, handler, target, customActionId)
        {
            _triggersType = new() { type };
            _applyHandler = applyHandler;
            _removeHandler = removeHandler;
        }

        public bool CanApply(BuffTriggerType type) =>
            _triggersType.Contains(type);

        public override void Apply()
        {
            if (CanApply(BuffTriggerType.Instant))
                _applyHandler(this, Handler.Cast.Caster, BuffTriggerType.Instant, default);
        }

        public void Apply(BuffTriggerType type, FightActor trigger, object? token = default)
        {
            if (CanApply(type))
                _applyHandler(this, trigger, type, token);
        }

        public override void Dispell() =>
            _removeHandler?.Invoke(this);
    }
}
