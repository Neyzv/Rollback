using Rollback.World.Game.Items.Types;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Handlers.Exchanges;

namespace Rollback.World.Game.Interactions.Dialogs.Exchanges.Trades.Traders
{
    public class PlayerTrader : Trader
    {
        public override int Id =>
            Character.Id;

        public Character Character { get; }

        public PlayerTrader(Character character)
            : base() =>
            Character = character;

        protected override PlayerItem? RetrieveFromStorageByUID(int uid) =>
            Character.Inventory.GetItemByUID(uid);

        public override void UpdateReadyState(int actorId, bool state) =>
            ExchangeHandler.SendExchangeIsReadyMessage(Character.Client, actorId, state);

        public override void UpdateKamas(int actorId, int amount) =>
            ExchangeHandler.SendExchangeKamaModifiedMessage(Character.Client, actorId != Id, amount);

        public override bool ChangeKamas(int amount)
        {
            if (amount >= 0 && amount <= Character.Kamas)
            {
                Kamas = amount;
                return true;
            }

            //Vous n\'avez pas assez de kamas pour effectuer cette action.
            Character.SendInformationMessage(Protocol.Enums.TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 82);

            return false;
        }

        public sealed override void UpdateItem(int actorId, TradeItem item, bool modified)
        {
            if (item.Quantity <= 0)
                ExchangeHandler.SendExchangeObjectRemovedMessage(Character.Client, actorId != Id, item.UID);
            else if (modified)
                ExchangeHandler.SendExchangeObjectModifiedMessage(Character.Client, actorId != Id, item.ObjectItem);
            else
                ExchangeHandler.SendExchangeObjectAddedMessage(Character.Client, actorId != Id, item.ObjectItem);
        }
    }
}
