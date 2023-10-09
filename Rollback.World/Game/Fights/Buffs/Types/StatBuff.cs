using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Effects.Handlers.Spells;
using Rollback.World.Game.Fights.Fighters;

namespace Rollback.World.Game.Fights.Buffs.Types
{
    public sealed class StatBuff : Buff
    {
        public short Amount { get; }

        public Stat Stat { get; }

        public override AbstractFightDispellableEffect AbstractFightDispellableEffect =>
            new FightTemporaryBoostEffect(Id, Target.Id, Duration, (sbyte)Handler.Dispellable, Handler.Cast.Spell.Id, Math.Abs(Amount));

        public StatBuff(int id, SpellEffectHandler handler, FightActor target, short amount, Stat stat, short? customActionId = null)
            : base(id, handler, target, customActionId)
        {
            Amount = amount;
            Stat = stat;
        }

        public override void Apply()
        {
            if (Target.Alive)
            {
                Target.Stats[Stat].Context += Amount;

                if (Target is CharacterFighter characterFighter)
                    characterFighter.Character.RefreshStats();
            }
        }

        public override void Dispell()
        {
            if (Target.Alive)
            {
                Target.Stats[Stat].Context -= Amount;

                if (Target is CharacterFighter characterFighter)
                    characterFighter.Character.RefreshStats();

                if (!Target.Alive)
                    Target.Kill(Target);
            }
        }
    }
}
