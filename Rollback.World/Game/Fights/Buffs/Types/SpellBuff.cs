using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Effects.Handlers.Spells;
using Rollback.World.Game.Fights.Fighters;

namespace Rollback.World.Game.Fights.Buffs.Types
{
    public sealed class SpellBuff : Buff
    {
        public short SpellId { get; }

        public short Amount { get; }

        public CharacterSpellModificationType Type { get; }

        public override AbstractFightDispellableEffect AbstractFightDispellableEffect =>
            new FightTemporarySpellBoostEffect(Id, Target.Id, Duration, (sbyte)Handler.Dispellable, Handler.Cast.Spell.Id, Amount, SpellId);

        public SpellBuff(int id, SpellEffectHandler handler, FightActor target, short spellId, short amount, short? customActionId = null) : base(id, handler, target, customActionId)
        {
            SpellId = spellId;
            Amount = amount;

            Type = Handler.Effect.Id switch
            {
                EffectId.EffectSpellBoost => CharacterSpellModificationType.BaseDamage,
                _ => CharacterSpellModificationType.None
            };
        }

        public override void Apply()
        {
            if (Target is CharacterFighter characterFighter)
                characterFighter.Character.AddSpellModification(Type, SpellId, Amount);
        }

        public override void Dispell()
        {
            if (Target is CharacterFighter characterFighter)
                characterFighter.Character.RemoveSpellModification(x => x.spellId == SpellId && x.modificationType == (sbyte)Type && x.value.contextModif == Amount);
        }
    }
}
