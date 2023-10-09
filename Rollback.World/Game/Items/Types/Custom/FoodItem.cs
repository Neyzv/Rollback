using Rollback.World.CustomEnums;
using Rollback.World.Database.Characters;
using Rollback.World.Game.Items.Attributes;
using Rollback.World.Game.Items.Storages;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.World.Maps;

namespace Rollback.World.Game.Items.Types.Custom
{
    [ItemType((int)ItemType.Pain), ItemType((int)ItemType.Potion)]
    public sealed class FoodItem : PlayerItem
    {
        public FoodItem(CharacterItemRecord record, Inventory inventory)
            : base(record, inventory) { }

        public override bool Use(Cell? targetedCell)
        {
            var toCheck = Effects.FirstOrDefault()?.Id switch
            {
                EffectId.EffectAddHealth => EffectId.EffectAddHealth,
                EffectId.EffectRestoreEnergyPoints => EffectId.EffectRestoreEnergyPoints,
                _ => default(EffectId?)
            };

            var target = targetedCell is null ? _storage.Owner : _storage.Owner.MapInstance.GetActor<Character>(x => x.Cell.Id == targetedCell.Id);

            return target is not null && (toCheck is null ||
                (toCheck is EffectId.EffectAddHealth && target.Stats.Health.Actual < target.Stats.Health.ActualMax) ||
                (toCheck is EffectId.EffectRestoreEnergyPoints && target.Energy < Character.MaxEnergy));
        }
    }
}
