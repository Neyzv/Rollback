using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Effects.Types;
using Rollback.World.Game.Fights;
using Rollback.World.Game.World.Maps.CellsZone;

namespace Rollback.World.Game.Effects.Handlers.Spells.Buffs
{
    [Identifier(EffectId.EffectSpellBoost)]
    public sealed class SpellBoostEffectHandler : SpellEffectHandler
    {
        public SpellBoostEffectHandler(EffectBase effect, List<FightActor> target, SpellCast cast, Zone zone) : base(effect, target, cast, zone) { }

        protected override void InternalApply(FightActor fighter)
        {
            if (Effect is EffectDice effectDice)
                fighter.AddSpellBuff(this, effectDice.DiceNum, effectDice.Value);
        }
    }
}
