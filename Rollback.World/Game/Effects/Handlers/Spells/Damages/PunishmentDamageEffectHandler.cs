using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Effects.Types;
using Rollback.World.Game.Fights;
using Rollback.World.Game.World.Maps.CellsZone;

namespace Rollback.World.Game.Effects.Handlers.Spells.Damages
{
    [Identifier(EffectId.EffectPunishmentDamage)]
    public sealed class PunishmentDamageEffectHandler : SpellEffectHandler
    {
        public PunishmentDamageEffectHandler(EffectBase effect, List<FightActor> target, SpellCast cast, Zone zone) : base(effect, target, cast, zone) { }

        protected override void InternalApply(FightActor fighter)
        {
            if (Effect is EffectDice effectDice)
                fighter.InflictDamage(Cast.Caster, new((short)(effectDice.DiceNum * Cast.Caster.Stats.Health.ActualMax *
                    (Math.Pow(Math.Cos(2 * Math.PI * (100 * Cast.Caster.Stats.Health.Actual / Cast.Caster.Stats.Health.ActualMax * 0.01 - 0.5)) + 1, 2) / 4) / 100), CustomEnums.EffectSchool.Neutral));
        }
    }
}
