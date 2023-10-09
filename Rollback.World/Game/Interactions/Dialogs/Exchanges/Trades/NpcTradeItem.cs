using Rollback.Protocol.Types;
using Rollback.World.Game.Effects;
using Rollback.World.Game.Items;
using Rollback.World.Game.Items.Types;

namespace Rollback.World.Game.Interactions.Dialogs.Exchanges.Trades
{
    public sealed class NpcTradeItem : TradeItem
    {
        public override ObjectItem ObjectItem
        {
            get
            {
                var objectItem = base.ObjectItem;
                objectItem.effects = EffectManager.GetObjectEffects(ItemManager.Instance.GetTemplateRecordById(_item.Id)!.Effects);

                return objectItem;
            }
        }

        public NpcTradeItem(PlayerItem item, int quantity)
            : base(item, quantity) { }
    }
}
