using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.World.Maps;

namespace Rollback.World.Game.Effects.Handlers.Items.Usable.Stats
{
    [Identifier(EffectId.EffectAddXp)]
    public sealed class AddExperienceUsableEffectHandler : UsableEffectHandler
    {
        public AddExperienceUsableEffectHandler(EffectBase effect, Character itemOwner, Cell targetedCell) : base(effect, itemOwner, targetedCell) { }

        public override void Apply()
        {
            if (GenerateEffect() is { } effectInteger && GetTarget() is { } target)
                target.ChangeExperience(effectInteger.Value);
        }
    }
}
