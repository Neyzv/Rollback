using Rollback.Common.DesignPattern.Threading.Schedul;
using Rollback.Common.DesignPattern.Threading.Schedul.Callback;
using Rollback.Protocol.Enums;
using Rollback.World.Game.Items.Storages;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.RolePlayActors.TaxCollectors;
using Rollback.World.Handlers.Exchanges;
using Rollback.World.Handlers.Inventory;

namespace Rollback.World.Game.Interactions.Dialogs.Exchanges.TaxCollectors
{
    internal class TaxCollectorExchange : Exchange<TaxCollector>
    {
        private IExecution? _closeExecution;

        public override ExchangeTypeEnum ExchangeType =>
            ExchangeTypeEnum.TAXCOLLECTOR;

        public TaxCollectorExchange(Character character, TaxCollector dialoger)
            : base(character, dialoger) { }

        protected override void InternalOpen()
        {
            Dialoger.Dialoger = Character;

            _closeExecution = Scheduler.Instance.ExecuteDelayed(Close)
                .WithTime(TimeSpan.FromMinutes(TaxCollectorManager.MinutesBeforeLeavingExchange));

            ExchangeHandler.SendExchangeStartedMessage(Character.Client, ExchangeType);
            InventoryHandler.SendStorageInventoryContentMessage(Character.Client, Dialoger.Bag.InventoryContent, Dialoger.GatheredKamas);
            //Attention, la fenêtre d\'échange se fermera automatiquement dans %1 minutes.
            Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 139, TaxCollectorManager.MinutesBeforeLeavingExchange);
        }

        protected override void InternalClose()
        {
            Scheduler.Instance.CancelExecutionDelayed(_closeExecution!);
            Dialoger.Dialoger = null;

            Dialoger.Guild.ChangeExperience(Dialoger.GatheredExperience);
            Character.GuildMember?.Fire(Dialoger.Id);

            ExchangeHandler.SendExchangeLeaveMessage(Character.Client, false);
        }

        public override void SetKamas(int actorId, int amount)
        {
            if (amount < 0 && Character.Kamas - amount <= Inventory.MaxKamasInInventory && Dialoger.GatheredKamas + amount >= 0)
            {
                Dialoger.ChangeGatheredKamas(amount);
                Character.ChangeKamas(-amount, false);

                ExchangeHandler.SendStorageKamasUpdateMessage(Character.Client, Dialoger.GatheredKamas);
            }
        }

        public override void MoveItem(int actorId, int uid, int quantity) =>
            Dialoger.Bag.MoveItemToCharacterInventory(Character, uid, -quantity);
    }
}
