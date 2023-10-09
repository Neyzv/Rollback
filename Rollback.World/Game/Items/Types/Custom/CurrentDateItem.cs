using Rollback.World.CustomEnums;
using Rollback.World.Database.Characters;
using Rollback.World.Game.Effects.Types;
using Rollback.World.Game.Items.Attributes;
using Rollback.World.Game.Items.Storages;

namespace Rollback.World.Game.Items.Types.Custom
{
    [ItemId(6867), ItemId(10289), ItemId(10290), ItemId(10291), ItemId(10292), ItemId(10293), ItemId(10294),
        ItemId(10295), ItemId(10296), ItemId(10297), ItemId(10298), ItemId(10299), ItemId(10300)]
    public sealed class CurrentDateItem : PlayerItem
    {
        public CurrentDateItem(CharacterItemRecord record, Inventory inventory)
            : base(record, inventory) =>
            Created += OnItemCreated;

        private void OnItemCreated(PlayerItem item)
        {
            if (Effects.All(x => x.Id is not EffectId.EffectReceiveOn))
                Effects.Add(new EffectDate(EffectId.EffectReceiveOn, DateTime.Now));
        }
    }
}
