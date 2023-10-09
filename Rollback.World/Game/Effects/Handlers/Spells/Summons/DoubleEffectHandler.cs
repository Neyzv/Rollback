using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Fights;
using Rollback.World.Game.Fights.Fighters;
using Rollback.World.Game.World.Maps.CellsZone;

namespace Rollback.World.Game.Effects.Handlers.Spells.Summons
{
    [Identifier(EffectId.EffectDouble)]
    public sealed class DoubleEffectHandler : SpellEffectHandler
    {
        public DoubleEffectHandler(EffectBase effect, List<FightActor> target, SpellCast cast, Zone zone) : base(effect, target, cast, zone) =>
            Target = new() { Cast.Caster };

        protected override void InternalApply(FightActor fighter)
        {
            var cell = Cast.TargetedCell;
            if (!fighter.Team!.Fight.IsCellFreeToWalkOn(cell.Id))
                cell = fighter.Team.Fight.GetFirstFreeCellNear(cell.Point);

            if (cell is not null)
                fighter.AddSummon(new SummonedClone(cell, fighter.Cell.Point.OrientationTo(cell.Point, false), fighter));
        }
    }
}
