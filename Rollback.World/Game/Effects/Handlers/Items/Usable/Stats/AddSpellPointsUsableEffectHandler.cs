using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.World.Maps;

namespace Rollback.World.Game.Effects.Handlers.Items.Usable.Stats
{
    [Identifier(EffectId.EffectAddSpellPoints)]
    public sealed class AddSpellPointsUsableEffectHandler : UsableEffectHandler
    {
        public AddSpellPointsUsableEffectHandler(EffectBase effect, Character itemOwner, Cell targetedCell) : base(effect, itemOwner, targetedCell) { }

        public override void Apply()
        {
            if (GetTarget() is { } target && GenerateEffect() is { } effectInteger)
                target.ChangeSpellPoints(effectInteger.Value);
        }
    }
}
