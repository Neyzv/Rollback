using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Fights;
using Rollback.World.Game.World.Maps.CellsZone;

namespace Rollback.World.Game.Effects.Handlers.Spells.Movements
{
    [Identifier(EffectId.EffectRepelsTo)]
    public sealed class RepealsToEffectHandler : SpellEffectHandler
    {
        public RepealsToEffectHandler(EffectBase effect, List<FightActor> target, SpellCast cast, Zone zone) : base(effect, target, cast, zone) =>
            Target = new() { Cast.Caster };

        protected override void InternalApply(FightActor fighter)
        {
            var orientation = fighter.Cell.Point.OrientationTo(Cast.TargetedCell.Point);
            var target = fighter.Team!.Fight.GetFighter<FightActor>(x => x.Cell.Id == fighter.Cell.Point.GetCellInDirection(orientation, 1)?.CellId);

            target?.Push(fighter, orientation, (short)target.Cell.Point.ManhattanDistanceTo(Cast.TargetedCell.Point), false);
        }
    }
}
