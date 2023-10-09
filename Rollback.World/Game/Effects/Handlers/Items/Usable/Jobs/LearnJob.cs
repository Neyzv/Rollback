using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.World.Maps;

namespace Rollback.World.Game.Effects.Handlers.Items.Usable.Jobs
{
    [Identifier(EffectId.LearnJob)]
    public sealed class LearnJob : UsableEffectHandler
    {
        public LearnJob(EffectBase effect, Character itemOwner, Cell targetedCell)
            : base(effect, itemOwner, targetedCell) { }

        public override void Apply()
        {
            if (GenerateEffect() is { } effectInteger && GetTarget() is { } character)
                character.LearnJob((JobIds)effectInteger.Value);
        }
    }
}
