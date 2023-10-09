﻿using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Fights;
using Rollback.World.Game.World.Maps.CellsZone;

namespace Rollback.World.Game.Effects.Handlers.Spells.Others
{
    [Identifier(EffectId.EffectThrow)]
    public sealed class ThrowEffectHandler : SpellEffectHandler
    {
        public ThrowEffectHandler(EffectBase effect, List<FightActor> target, SpellCast cast, Zone zone) : base(effect, target, cast, zone) =>
            Target = new() { Cast.Caster };

        protected override void InternalApply(FightActor fighter) =>
            fighter.Throw(Cast.TargetedCell);
    }
}
