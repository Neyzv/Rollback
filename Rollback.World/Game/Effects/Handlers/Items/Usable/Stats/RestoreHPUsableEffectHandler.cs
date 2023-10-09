using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.World.Maps;

namespace Rollback.World.Game.Effects.Handlers.Items.Usable.Stats
{
    [Identifier(EffectId.EffectAddHealth)]
    public sealed class RestoreHPUsableEffectHandler : UsableEffectHandler
    {
        public RestoreHPUsableEffectHandler(EffectBase effect, Character itemOwner, Cell targetedCell)
            : base(effect, itemOwner, targetedCell) { }

        public override void Apply()
        {
            var effectInteger = GenerateEffect();
            if (effectInteger is not null)
            {
                var target = GetTarget();
                if (target is not null && target.Stats.Health.Actual < target.Stats.Health.ActualMax)
                    target.RegainLife(effectInteger.Value);
            }
        }
    }
}
