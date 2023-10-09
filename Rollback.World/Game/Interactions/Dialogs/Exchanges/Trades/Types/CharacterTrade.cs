using Rollback.Protocol.Enums;
using Rollback.World.Game.Interactions.Dialogs.Exchanges.Trades.Traders;
using Rollback.World.Game.Items.Storages;
using Rollback.World.Handlers.Exchanges;

namespace Rollback.World.Game.Interactions.Dialogs.Exchanges.Trades.Types
{
    public sealed class CharacterTrade : Trade<PlayerTrader>
    {
        public override ExchangeTypeEnum ExchangeType =>
            ExchangeTypeEnum.PLAYER_TRADE;

        public CharacterTrade(PlayerTrader sender, PlayerTrader receiver) : base(sender, receiver) { }

        protected override void InternalOpen()
        {
            ExchangeHandler.SendExchangeStartedMessage(Sender.Character.Client, ExchangeType);

            Receiver.Character.Interaction = this;
            ExchangeHandler.SendExchangeStartedMessage(Receiver.Character.Client, ExchangeType);
        }

        protected override void InternalClose()
        {
            Receiver.Character.Interaction = default;
            ExchangeHandler.SendExchangeLeaveMessage(Sender.Character.Client, false);
        }

        protected override void InternalProcessTrade()
        {
            if (Sender.Character.Kamas + Receiver.Kamas - Sender.Kamas <= Inventory.MaxKamasInInventory &&
                Receiver.Character.Kamas + Sender.Kamas - Receiver.Kamas <= Inventory.MaxKamasInInventory &&
                Receiver.Character.Inventory.Pods + Sender.Items.Sum(x => x.StackPod) <= Receiver.Character.Inventory.MaxPods &&
                Sender.Character.Inventory.Pods + Receiver.Items.Sum(x => x.StackPod) <= Sender.Character.Inventory.MaxPods)
            {
                Sender.Character.ChangeKamas(Receiver.Kamas - Sender.Kamas, false);
                Receiver.Character.ChangeKamas(Sender.Kamas - Receiver.Kamas, false);

                foreach (var item in Sender.Items)
                    Sender.Character.Inventory.ChangeItemOwner(Receiver.Character.Inventory, item.UID, item.Quantity);

                foreach (var item in Receiver.Items)
                    Receiver.Character.Inventory.ChangeItemOwner(Sender.Character.Inventory, item.UID, item.Quantity);
            }
        }
    }
}
