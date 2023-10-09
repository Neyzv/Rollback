using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Fights;
using Rollback.World.Game.World.Maps.CellsZone;

namespace Rollback.World.Game.Effects.Handlers.Spells.Others
{
    [Identifier(EffectId.EffectCarry)]
    internal class CarryEffectHandler : SpellEffectHandler
    {
        private bool _carryActor;

        public CarryEffectHandler(EffectBase effect, List<FightActor> target, SpellCast cast, Zone zone) : base(effect, target, cast, zone)
        {
        }

        protected override void InternalApply(FightActor fighter)
        {
            if (!_carryActor)
            {
                Cast.Caster.Carry(fighter, this);
                _carryActor = true;
            }
        }
    }
}
