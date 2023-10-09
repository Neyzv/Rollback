using Rollback.Protocol.Enums;
using Rollback.Protocol.Types;
using Rollback.World.Game.Effects.Handlers.Spells;

namespace Rollback.World.Game.Fights.Buffs.Types
{
    public sealed class InvisibilityBuff : Buff
    {
        public override AbstractFightDispellableEffect AbstractFightDispellableEffect =>
            new FightTemporaryBoostEffect(Id, Target.Id, Duration, (sbyte)Handler.Dispellable, Handler.Cast.Spell.Id, 1);

        public InvisibilityBuff(int id, SpellEffectHandler handler, FightActor target, short? customActionId = null) : base(id, handler, target, customActionId) { }

        public override void Apply() =>
            Target.SetVisibleState(GameActionFightInvisibilityStateEnum.INVISIBLE, Handler.Cast.Caster);

        public override void Dispell()
        {
            if (Target.GetBuffs<InvisibilityBuff>().Length is 0)
                Target.SetVisibleState(GameActionFightInvisibilityStateEnum.VISIBLE, Handler.Cast.Caster);
        }
    }
}
