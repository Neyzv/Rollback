using System.Diagnostics.CodeAnalysis;
using Rollback.Protocol.Enums;
using Rollback.World.Game.Interactions.Dialogs.Exchanges.Trades.Traders;
using Rollback.World.Handlers.Exchanges;

namespace Rollback.World.Game.Interactions.Dialogs.Exchanges.Trades
{
    public abstract class Trade<T> : ITrade
        where T : Trader
    {
        public abstract ExchangeTypeEnum ExchangeType { get; }

        public PlayerTrader Sender { get; }

        public T Receiver { get; }

        public Trade(PlayerTrader sender, T receiver)
        {
            Sender = sender;
            Receiver = receiver;
        }

        public event Action<Trader, TradeItem, bool>? ItemMoved;
        public event Action<Trader, int>? KamasChanged;

        protected abstract void InternalOpen();

        public void Open()
        {
            Sender.Character.Interaction = this;
            InternalOpen();
        }

        protected virtual void InternalClose() { }

        public void Close()
        {
            Sender.Character.Interaction = default;
            ExchangeHandler.SendExchangeLeaveMessage(Sender.Character.Client, false);
            InternalClose();
        }

        protected virtual void InternalProcessTrade() { }

        protected virtual void ProcessTrade()
        {
            InternalProcessTrade();

            Close();
        }

        public void ChangeReadyState(int actorId, bool state)
        {
            if (AssignSender(actorId, out var actionSender) && state != actionSender!.Ready)
            {
                actionSender.Ready = state;

                if (actorId > 0)
                {
                    Sender.UpdateReadyState(actorId, state);
                    Receiver.UpdateReadyState(actorId, state);
                }

                if (Sender.Ready && Receiver.Ready)
                    ProcessTrade();
            }
        }

        private bool AssignSender(int actorId, [NotNullWhen(true)] out Trader? actionSender)
        {
            actionSender = null;

            if (actorId == Sender.Id)
                actionSender = Sender;
            else if (actorId == Receiver.Id)
                actionSender = Receiver;

            return actionSender is not null;
        }

        public void SetKamas(int actorId, int quantity)
        {
            ChangeReadyState(Sender.Id, false);
            ChangeReadyState(Receiver.Id, false);

            if (AssignSender(actorId, out var actionSender) && actionSender.ChangeKamas(quantity))
            {
                Sender.UpdateKamas(actorId, quantity);
                Receiver.UpdateKamas(actorId, quantity);

                KamasChanged?.Invoke(actionSender, quantity);
            }
        }

        protected void ClearItems(Trader trader)
        {
            foreach (var item in trader.Items.ToArray())
                MoveItem(trader.Id, item.UID, -item.Quantity);

            trader.OnClearItems();
        }

        public void MoveItem(int actorId, int uid, int quantity)
        {
            ChangeReadyState(Sender.Id, false);
            ChangeReadyState(Receiver.Id, false);

            if (AssignSender(actorId, out var actionSender) &&
                actionSender.MoveItem(uid, quantity, out var item, out var modified))
            {
                Sender.UpdateItem(actorId, item, modified);
                Receiver.UpdateItem(actorId, item, modified);

                ItemMoved?.Invoke(actionSender, item, modified);
            }
        }
    }
}
