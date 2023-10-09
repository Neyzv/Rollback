using Rollback.Common.Network.IPC.Messages.Accounts;

namespace Rollback.World.Network.IPC.Handlers.Accounts
{
    public static class IPCAccountHandler
    {
        public static void SendAccountBannedMessage(int accountId, int banEndTime) =>
            IPCService.Instance.Send(new AccountBannedMessage(accountId, banEndTime));
    }
}
