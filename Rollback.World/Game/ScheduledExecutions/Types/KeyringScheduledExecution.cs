using Rollback.Common.DesignPattern.Threading.Schedul;
using Rollback.Common.DesignPattern.Threading.Schedul.Callback;
using Rollback.World.Game.Items.Types.Custom;
using Rollback.World.Game.ScheduledExecutions.Types.Abstractions;
using Rollback.World.Network;

namespace Rollback.World.Game.ScheduledExecutions.Types
{
    public sealed class KeyringScheduledExecution : BaseScheduledExecution
    {
        private const string Id = "Keyring";

        public override string Identifier =>
            Id;

        private void ResetKeyRing()
        {
            foreach (var client in WorldServer.Instance.GetClients(x => x.Account?.Character is not null))
            {
                foreach (var keyring in client.Account!.Character!.Inventory.GetItems(x => x.Id == KeyringItem.KeyringId))
                    if (keyring is KeyringItem keyringItem)
                        keyringItem.Reset();

                client.Account.Character.Inventory.Refresh();
            }
        }

        public override IExecution Start() =>
            Scheduler.Instance.ExecutePeriodically(ResetKeyRing)
                .WithDays(DayOfWeek.Monday);
    }
}
