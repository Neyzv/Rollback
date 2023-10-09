using Rollback.World.Game.Interactions.Dialogs.Exchanges.Trades.Types;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Handlers.Exchanges;

namespace Rollback.World.Game.Interactions.Requests.Exchanges
{
    public sealed class ExchangeRequest : Request
    {
        public ExchangeRequest(Character sender, Character receiver) : base(sender, receiver)
        {
        }

        protected override void InternalOpen()
        {
            ExchangeHandler.SendExchangeRequestedTradeMessage(Sender.Client, Protocol.Enums.ExchangeTypeEnum.PLAYER_TRADE, Sender.Id, Receiver.Id);
            ExchangeHandler.SendExchangeRequestedTradeMessage(Receiver.Client, Protocol.Enums.ExchangeTypeEnum.PLAYER_TRADE, Sender.Id, Receiver.Id);
        }

        protected override void InternalAccept() =>
            new CharacterTrade(new(Sender), new(Receiver)).Open();

        protected override void InternalClose()
        {
            ExchangeHandler.SendExchangeLeaveMessage(Sender.Client, false);
            ExchangeHandler.SendExchangeLeaveMessage(Receiver.Client, false);
        }
    }
}
