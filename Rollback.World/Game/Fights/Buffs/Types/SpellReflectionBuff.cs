using Rollback.Protocol.Types;
using Rollback.World.Game.Effects.Handlers.Spells;
using Rollback.World.Game.Effects.Types;
using Rollback.World.Game.Spells;

namespace Rollback.World.Game.Fights.Buffs.Types
{
    public sealed class SpellReflectionBuff : Buff
    {
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
                    0,
                    values.Length > 0 ? Convert.ToInt32(values[0]) : 0,
                    values.Length > 1 ? Convert.ToInt32(values[1]) : 0,
                    values.Length > 2 ? Convert.ToInt32(values[2]) : 0);
            }
        }

        public SpellReflectionBuff(int id, SpellEffectHandler handler, FightActor target, short? customActionId = null)
            : base(id, handler, target, customActionId) { }

        public bool Reflect(Spell spell) =>
            spell.Id is not 0 && Handler.Effect is EffectDice effectDice && spell.Level <= effectDice.DiceFace && (effectDice.Value >= 100 || effectDice.Value >= Random.Shared.Next(101));
    }
}
