using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Fights;
using Rollback.World.Game.World.Maps;
using Rollback.World.Game.World.Maps.CellsZone;
using Rollback.World.Game.World.Maps.CellsZone.Shapes;

namespace Rollback.World.Game.Effects.Handlers.Spells.Movements
{
    [Identifier(EffectId.EffectPushBack), Identifier(EffectId.EffectPullForward)]
    public sealed class PushEffectHandler : SpellEffectHandler
    {
        private readonly Dictionary<int, MapPoint> _startPostions;
        private readonly bool _pull;
        private readonly Cell _referenceCell;

        public PushEffectHandler(EffectBase effect, List<FightActor> target, SpellCast cast, Zone zone) : base(effect, target, cast, zone)
        {
            _pull = Effect.Id is EffectId.EffectPullForward;
            if (_pull)
                Target.Reverse();

            _startPostions = new();
            foreach (var t in Target)
                _startPostions[t.Id] = t.Cell.Point;

            _referenceCell = zone is Lozenge or Cross ? zone.CenterCell : Cast.Caster.Cell;
        }

        protected override void InternalApply(FightActor fighter)
        {
            var effectInteger = GenerateEffect();
            if (effectInteger is not null)
            {
                // TO DO can be pushed and carrying actor
                if (_referenceCell.Id != fighter.Cell.Id && _startPostions.ContainsKey(fighter.Id))
                    fighter.Push(Cast.Caster, _pull ? _startPostions[fighter.Id].OrientationTo(_referenceCell.Point)
                        : _referenceCell.Point.OrientationTo(_startPostions[fighter.Id]),
                        effectInteger.Value, _pull);
            }
        }
    }
}
