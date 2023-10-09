using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.World.Maps;

namespace Rollback.World.Game.Effects.Handlers.Items.Usable
{
    public abstract class UsableEffectHandler : EffectHandler
    {
        private readonly Character _itemOwner;

        public Cell TargetedCell { get; }

        protected UsableEffectHandler(EffectBase effect, Character itemOwner, Cell targetedCell)
            : base(effect)
        {
            _itemOwner = itemOwner;
            TargetedCell = targetedCell;
        }

        protected Character? GetTarget() =>
            _itemOwner.Cell.Id == TargetedCell.Id ? _itemOwner : _itemOwner.MapInstance.GetActor<Character>(x => x.Cell.Id == TargetedCell.Id);
    }
}
