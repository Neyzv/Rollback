using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Effects.Handlers.Items
{
    public abstract class ItemEffectHandler : TargetedEffectHandler<Character>
    {
        public ItemEffectHandler(EffectBase effect, Character target) : base(effect, target) { }

        public abstract void UnApply();
    }
}
